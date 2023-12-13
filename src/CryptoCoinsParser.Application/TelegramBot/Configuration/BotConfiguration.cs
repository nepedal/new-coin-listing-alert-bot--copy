namespace CryptoCoinsParser.Application.TelegramBot.Configuration;

public sealed record BotConfiguration
{
    public const string Section = "Bot";

    public string Token { get; init; }

    public short MaxConnections { get; init; }

    public string ContactNickname { get; set; }
}
