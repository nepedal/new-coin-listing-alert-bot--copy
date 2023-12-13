namespace CryptoCoinsParser.Application.Logging;

public sealed class TelegramLoggerProvider : ILoggerProvider
{
    private readonly TelegramLoggerConfiguration _configuration;

    private readonly ConcurrentDictionary<string, TelegramLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

    public TelegramLoggerProvider(TelegramLoggerConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, new TelegramLogger(_configuration, categoryName));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
