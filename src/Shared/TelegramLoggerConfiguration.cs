using Microsoft.Extensions.Logging;

namespace Shared;

public sealed record TelegramLoggerConfiguration
{
    public const string Section = "Telegram";

    public Dictionary<string, LogLevel> LogLevel { get; set; }

    public string AccessToken { get; set; }

    public string ChatId { get; set; }

    public bool Enabled { get; set; }
}
