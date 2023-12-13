using User = CryptoCoinsParser.Domain.DbEntities.User;

namespace CryptoCoinsParser.Application.HostedServices;

public class NotifyAnnouncementHostedService : BackgroundService
{
    private static readonly TimeSpan DelayTime = TimeSpan.FromMinutes(10);

    private readonly ITelegramBotClient _bot;

    private readonly ILogger<NotifyAnnouncementHostedService> _logger;

    private readonly IServiceProvider _services;

    public NotifyAnnouncementHostedService(ILogger<NotifyAnnouncementHostedService> logger,
        IServiceProvider services,
        ITelegramBotClient bot)
    {
        _logger = logger;
        _services = services;
        _bot = bot;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _services.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                var synchronizeAnnouncementService =
                    scope.ServiceProvider.GetRequiredService<ISynchronizeAnnouncementService>();

                var announcements = await synchronizeAnnouncementService.GetAnnouncementsAsync();

                if (!announcements.Any())
                {
                    await Task.Delay(DelayTime, stoppingToken);
                    continue;
                }

                var users = await userRepository.GetUsersAsync();

                foreach (var announcement in announcements)
                {
                    await SendMessagesAsync(announcement, users);
                }
            }
            catch (TaskCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // do nothing
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occur in {MS}", nameof(NotifyAnnouncementHostedService));
            }
        }
    }

    private async Task SendMessagesAsync(Announcement announcement, List<User> users)
    {
        var chartService = _services.GetRequiredService<IChartService>();

        foreach (var user in users)
        foreach (var coin in announcement.Coins)
        {
            string message;

            if (coin.Announcements.Count == 1)
            {
                message = $"{announcement.Exchange.ToString()} to LIST ({coin.Name}-USDT) for Spot Trading 🔔\n\n" +
                          "⏰ Act fast and do your research! Always trade responsibly.\n\n" +
                          $"📅 Date: {DateTime.Now:dd/MM/yyyy}";

                await _bot.SendTextMessageAsync(new ChatId(user.TelegramUserId), message, parseMode: ParseMode.Html);

                await Task.Delay(TimeSpan.FromMilliseconds(300));

                continue;
            }

            var coinExchanges = coin.Announcements.Select(x => x.Exchange.ToString()).Distinct().ToList();

            message = $"{announcement.Exchange.ToString()} to LIST ({coin.Name}-USDT) for Spot Trading 🔔\n\n" +
                      $"📈 {coin.Name} Previous Listings spotted by this bot:";

            foreach (var exchange in coinExchanges)
            {
                message += $"\n- {exchange}";
            }

            message += $"\n\n📊 Check out how {coin.Name} performed in the first 30 mins on other exchanges" +
                       "\n\n⏰ Act fast and do your research! Always trade responsibly." +
                       $"\n\n📅 Date: {DateTime.Now:dd/MM/yyyy}";

            var images = new List<InputMediaPhoto>();

            foreach (var coinAnnouncement in coin.Announcements.Where(coinAnnouncement =>
                         coinAnnouncement.Id != announcement.Id))
            {
                images.Add(await chartService.GenerateChartAsync(coinAnnouncement.Exchange, coin));
            }

            if (images.Any())
            {
                images[0].Caption = message;
                images[0].ParseMode = ParseMode.Html;

                await _bot.SendMediaGroupAsync(new ChatId(user.TelegramUserId), images);
            }
            else
            {
                await _bot.SendTextMessageAsync(new ChatId(user.TelegramUserId), message, parseMode: ParseMode.Html);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(300));
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{BgService} is stopping", nameof(NotifyAnnouncementHostedService));

        await base.StopAsync(cancellationToken);
    }
}
