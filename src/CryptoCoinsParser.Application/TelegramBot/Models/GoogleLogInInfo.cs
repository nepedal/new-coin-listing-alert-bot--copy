namespace CryptoCoinsParser.Application.TelegramBot.Models;

public sealed record GoogleLogInInfo
{
    [JsonRequired]
    public string LogInUrl { get; set; }

    [JsonRequired]
    public string RequestId { get; set; }
}
