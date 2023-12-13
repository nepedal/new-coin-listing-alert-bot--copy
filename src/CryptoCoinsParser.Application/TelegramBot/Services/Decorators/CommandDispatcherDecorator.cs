namespace CryptoCoinsParser.Application.TelegramBot.Services.Decorators;

public sealed class CommandDispatcherDecorator : ICommandDispatcher
{
    private readonly ITelegramBotRequestLimiterService _botRequestLimiterService;

    private readonly ICommandDispatcher _commandDispatcherImplementation;

    private readonly ITelegramBotClient _telegramBotClient;

    public CommandDispatcherDecorator(ICommandDispatcher commandDispatcherImplementation,
        ITelegramBotRequestLimiterService botRequestLimiterService,
        ITelegramBotClient telegramBotClient)
    {
        _commandDispatcherImplementation = commandDispatcherImplementation;
        _botRequestLimiterService = botRequestLimiterService;
        _telegramBotClient = telegramBotClient;
    }

    public Task DispatchAsync(Update update)
    {
        return _commandDispatcherImplementation.DispatchAsync(update);
    }

    public async Task ExecuteCommandHandler(IBotCommandHandler commandHandler, Update update)
    {
        var chat = update.GetUpdateChat();

        var isAllowedRequests = await _botRequestLimiterService.IsAllowedRequestsAsync(chat.Id);

        if (!isAllowedRequests)
        {
            await _botRequestLimiterService.StopGettingRequestsAsync(chat.Id);
            await _commandDispatcherImplementation.ExecuteCommandHandler(commandHandler, update);
            await _botRequestLimiterService.AllowGettingRequestsAsync(chat.Id);
        }
        else
        {
            await _telegramBotClient.SendTextMessageAsync(chat.Id, RequestLimitMessage);
        }
    }
}
