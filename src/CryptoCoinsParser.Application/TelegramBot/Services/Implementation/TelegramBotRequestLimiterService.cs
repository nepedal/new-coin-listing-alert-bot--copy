using CryptoCoinsParser.Persistence.Cache;
using CryptoCoinsParser.Persistence.Cache.Models;

namespace CryptoCoinsParser.Application.TelegramBot.Services.Implementation;

public sealed class TelegramBotRequestLimiterService : ITelegramBotRequestLimiterService
{
    private readonly IKeyValueRepository<bool> _requestLimiter;

    public TelegramBotRequestLimiterService(IKeyValueRepository<bool> requestLimiter)
    {
        _requestLimiter = requestLimiter;
    }

    public Task StopGettingRequestsAsync(long telegramUserId)
    {
        var cacheKey = new CachedRequestLimiterKey(telegramUserId);

        return _requestLimiter.CreateOrUpdateAsync(cacheKey, true);
    }

    public Task AllowGettingRequestsAsync(long telegramUserId)
    {
        var cacheKey = new CachedRequestLimiterKey(telegramUserId);

        return _requestLimiter.DeleteAsync(cacheKey);
    }

    public Task<bool> IsAllowedRequestsAsync(long telegramUserId)
    {
        var cacheKey = new CachedRequestLimiterKey(telegramUserId);

        return _requestLimiter.GetByKeyAsync(cacheKey);
    }
}
