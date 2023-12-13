using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CryptoCoinsParser.Persistence.Cache;

public class KeyValueRepository<T> : IKeyValueRepository<T>
{
    private readonly IDistributedCache _cacheClient;

    public KeyValueRepository(IDistributedCache cacheClient)
    {
        _cacheClient = cacheClient;
    }

    public async Task<T> GetByKeyAsync(IKey key)
    {
        var value = await _cacheClient.GetStringAsync(key.GetStringKey());

        return value != null ? JsonConvert.DeserializeObject<T>(value) : default;
    }

    public Task CreateOrUpdateAsync(IKey key, T value)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(10)
        };

        return _cacheClient.SetStringAsync(key.GetStringKey(), JsonConvert.SerializeObject(value), options);
    }

    public Task DeleteAsync(IKey key)
    {
        return _cacheClient.RemoveAsync(key.GetStringKey());
    }
}
