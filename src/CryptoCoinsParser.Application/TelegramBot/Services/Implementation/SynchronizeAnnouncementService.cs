namespace CryptoCoinsParser.Application.TelegramBot.Services.Implementation;

public class SynchronizeAnnouncementService : ISynchronizeAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;

    private readonly BinanceScraperService _binanceScraperService;

    private readonly BybitScraperService _bybitScraperService;

    private readonly ICoinRepository _coinRepository;

    private readonly KuCoinScraperService _kuCoinScraperService;

    private readonly OkxScraperService _okxScraperService;

    public SynchronizeAnnouncementService(IAnnouncementRepository announcementRepository,
        KuCoinScraperService kuCoinScraperService,
        OkxScraperService okxScraperService,
        BybitScraperService bybitScraperService,
        BinanceScraperService binanceScraperService,
        ICoinRepository coinRepository)
    {
        _announcementRepository = announcementRepository;
        _kuCoinScraperService = kuCoinScraperService;
        _okxScraperService = okxScraperService;
        _bybitScraperService = bybitScraperService;
        _binanceScraperService = binanceScraperService;
        _coinRepository = coinRepository;
    }

    public async Task<List<Announcement>> GetAnnouncementsAsync()
    {
        var announcements = new List<AnnouncementDto>();

        announcements.AddRange(await _binanceScraperService.GetLatest());
        announcements.AddRange(await _bybitScraperService.GetLatest());
        announcements.AddRange(await _okxScraperService.GetLatest());
        announcements.AddRange(await _kuCoinScraperService.GetLatest());

        var oldAnnouncements = await _announcementRepository.GetAnnouncementAsync();

        var newAnnouncements = new List<AnnouncementDto>();
        newAnnouncements.AddRange(announcements.Where(announcement =>
            oldAnnouncements.All(c => c.Message != announcement.Message)));

        var synchronizedAnnouncements = await SynchronizeAnnouncements(newAnnouncements);

        return synchronizedAnnouncements;
    }

    public async Task<List<Announcement>> SynchronizeAnnouncements(IEnumerable<AnnouncementDto> announcementDtos)
    {
        var announcements = new List<Announcement>();
        foreach (var announcement in announcementDtos)
        {
            var replacedCoins = new List<Coin>();

            foreach (var coin in announcement.Coins)
            {
                var existingCoin = await _coinRepository.GetCoinByNameAsync(coin.Name);

                replacedCoins.Add(existingCoin ?? new Coin { Name = coin.Name });
            }

            var synchronizedAnnouncement = new Announcement
            {
                Message = announcement.Message,
                Coins = replacedCoins,
                Exchange = announcement.Exchange
            };

            announcements.Add(synchronizedAnnouncement);
        }

        await _announcementRepository.CreateAnnouncementRangeAsync(announcements);

        return announcements;
    }
}
