namespace CryptoCoinsParser.Api.HostedServices;

public class ConfigureWebhookHostedService : IHostedService
{
    private readonly ApplicationConfiguration _applicationConfiguration;

    private readonly BotConfiguration _botConfig;

    private readonly ILogger<ConfigureWebhookHostedService> _logger;

    private readonly IServiceProvider _services;

    public ConfigureWebhookHostedService(ILogger<ConfigureWebhookHostedService> logger,
        IServiceProvider serviceProvider,
        BotConfiguration botConfig,
        ApplicationConfiguration applicationConfiguration)
    {
        _logger = logger;
        _services = serviceProvider;
        _botConfig = botConfig;
        _applicationConfiguration = applicationConfiguration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Configure custom endpoint per Telegram API recommendations:
        // https://core.telegram.org/bots/api#setwebhook
        // If you'd like to make sure that the Webhook request comes from Telegram, we recommend
        // using a secret path in the URL, e.g. https://www.example.com/<token>.
        // Since nobody else knows your bot's token, you can be pretty sure it's us.
        var webhookAddress = @$"{_applicationConfiguration.ServerUrl}/handle-command";
        _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);

        await botClient.SetWebhookAsync(
            webhookAddress,
            allowedUpdates: new List<UpdateType>(),
            cancellationToken: cancellationToken,
            maxConnections: _botConfig.MaxConnections);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
