namespace CryptoCoinsParser.Application.Logging;

public sealed class TelegramLogger : ILogger
{
    private const int MaxLengthOfTelegramMessage = 4096;

    private readonly ITelegramBotClient _botClient;

    private readonly string _categoryName;

    private readonly TelegramLoggerConfiguration _configuration;

    public TelegramLogger(TelegramLoggerConfiguration configuration, string categoryName) : this(
        new TelegramBotClient(configuration.AccessToken), categoryName)
    {
        _configuration = configuration;
    }

    private TelegramLogger(ITelegramBotClient client, string categoryName)
    {
        _botClient = client;
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return default!;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _configuration.LogLevel.ContainsValue(logLevel);
    }

    public void Log<TState>(LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = GetMessage(logLevel, formatter(state, exception));

        if (message.Length >= MaxLengthOfTelegramMessage)
        {
            message = message[..MaxLengthOfTelegramMessage];
        }

        _botClient.SendTextMessageAsync(_configuration.ChatId, message, parseMode: ParseMode.Html);
    }

    private string GetMessage(LogLevel level, string logMessage)
    {
        var currentEnv = Environment.GetEnvironmentVariable(NetConstants.AspnetcoreEnvironment);
        var levelEmoji = level switch
        {
            LogLevel.Trace => "⬜️",
            LogLevel.Debug => "🟦",
            LogLevel.Information => "⬛️️️",
            LogLevel.Warning => "🟧",
            LogLevel.Error => "🟥",
            LogLevel.Critical => "❌",
            LogLevel.None => "🔳",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        return Concat("Environment: <b>", currentEnv, "</b>\r\n", levelEmoji, level, "\r\n<b>",
            _categoryName, "</b>\r\n\r\n", "Message:", logMessage);
    }
}
