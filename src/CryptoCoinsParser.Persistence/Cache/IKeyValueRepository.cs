namespace CryptoCoinsParser.Persistence.Cache;

public interface IKeyValueRepository<T>
{
    Task<T> GetByKeyAsync(IKey key);

    Task CreateOrUpdateAsync(IKey key, T value);

    Task DeleteAsync(IKey key);
}
