namespace CryptoCoinsParser.Persistence.Cache.Models;

public sealed record CachedRequestLimiterKey : IKey
{
    private const string RequestLimiterKey = "RequestLimiter";

    private readonly long _telegramUserId;

    public CachedRequestLimiterKey(long telegramUser)
    {
        _telegramUserId = telegramUser;
    }

    public string GetStringKey()
    {
        return $"{RequestLimiterKey}_{_telegramUserId}";
    }
}
