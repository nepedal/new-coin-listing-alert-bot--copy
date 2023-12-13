using CryptoCoinsParser.Persistence.Services;

var provider = ContainerConfiguration.Configure();
var commandDispatcher = provider.GetService<ICommandDispatcher>();
var botConfiguration = provider.GetService<BotConfiguration>();

using (var scope = provider.CreateScope())
{
    var migrationRunner = scope.ServiceProvider.GetService<IAutomaticDbMigrationService>() ??
                          throw new ArgumentNullException($"{typeof(IAutomaticDbMigrationService)} can`t be null");

    migrationRunner.Run();

    await scope.ServiceProvider.GetRequiredService<ITelegramBotClient>().DeleteWebhookAsync();
}

ArgumentNullException.ThrowIfNull(botConfiguration);

var botClient = new TelegramBotClient(botConfiguration.Token);

using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};

botClient.StartReceiving(
    HandleUpdateAsync,
    HandlePollingErrorAsync,
    receiverOptions,
    cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is null && update.CallbackQuery is null)
    {
        return;
    }

    await commandDispatcher.DispatchAsync(update)!;
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var errorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(errorMessage);
    return Task.CompletedTask;
}
