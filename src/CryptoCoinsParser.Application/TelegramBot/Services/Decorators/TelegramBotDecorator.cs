using Telegram.Bot.Requests.Abstractions;

namespace CryptoCoinsParser.Application.TelegramBot.Services.Decorators;

internal sealed class TelegramBotDecorator : ITelegramBotClient
{
    private readonly ILogger<TelegramBotDecorator> _logger;

    private readonly IServiceProvider _services;

    private readonly ITelegramBotClient _telegramBotClientImplementation;

    public TelegramBotDecorator(ITelegramBotClient telegramBotClientImplementation,
        ILogger<TelegramBotDecorator> logger,
        IServiceProvider services)
    {
        _telegramBotClientImplementation = telegramBotClientImplementation;
        _logger = logger;
        _services = services;
    }

    public async Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = new())
    {
        var response = default(TResponse);

        try
        {
            response = await _telegramBotClientImplementation.MakeRequestAsync(request, cancellationToken);
        }
        catch (ApiRequestException e) when (e.ErrorCode == StatusCodes.Status403Forbidden)
        {
            var telegramUserId = GetTelegramUserId(request);
            using var scope = _services.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            await userRepository.DeleteUserByTelegramUserIdAsync(telegramUserId!.Value);
        }
        catch (Exception e)
        {
            var requestAsString = JsonConvert.SerializeObject(request);

            _logger.LogError(e, "Exception occured while making api request: (request = {Request}), {ExMessage}",
                requestAsString, e.Message);
        }

        return response;
    }

    public Task<bool> TestApiAsync(CancellationToken cancellationToken = new())
    {
        return _telegramBotClientImplementation.TestApiAsync(cancellationToken);
    }

    public Task DownloadFileAsync(string filePath,
        Stream destination,
        CancellationToken cancellationToken = new())
    {
        return _telegramBotClientImplementation.DownloadFileAsync(filePath, destination, cancellationToken);
    }

    public bool LocalBotServer => _telegramBotClientImplementation.LocalBotServer;

    public long? BotId => _telegramBotClientImplementation.BotId;

    public TimeSpan Timeout
    {
        get => _telegramBotClientImplementation.Timeout;

        set => _telegramBotClientImplementation.Timeout = value;
    }

    public IExceptionParser ExceptionsParser
    {
        get => _telegramBotClientImplementation.ExceptionsParser;

        set => _telegramBotClientImplementation.ExceptionsParser = value;
    }

    public event AsyncEventHandler<ApiRequestEventArgs> OnMakingApiRequest
    {
        add => _telegramBotClientImplementation.OnMakingApiRequest += value;

        remove => _telegramBotClientImplementation.OnMakingApiRequest -= value;
    }

    public event AsyncEventHandler<ApiResponseEventArgs> OnApiResponseReceived
    {
        add => _telegramBotClientImplementation.OnApiResponseReceived += value;

        remove => _telegramBotClientImplementation.OnApiResponseReceived -= value;
    }

    private static long? GetTelegramUserId(IRequest request)
    {
        return request switch
        {
            SendMessageRequest sendMessageRequest => sendMessageRequest.ChatId.Identifier,
            EditMessageTextRequest editMessageTextRequest => editMessageTextRequest.ChatId.Identifier,
            SendVideoRequest sendVideoRequest => sendVideoRequest.ChatId.Identifier,
            // SendMediaGroupRequest sendMediaGroupRequest => sendMediaGroupRequest.ChatId.Identifier, 
            _ => default
        };
    }
}
