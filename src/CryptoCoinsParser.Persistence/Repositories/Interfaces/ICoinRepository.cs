namespace CryptoCoinsParser.Persistence.Repositories.Interfaces;

public interface ICoinRepository
{
    Task<Coin> GetCoinAsync(long id);

    Task CreateCoinAsync(Coin coin);

    Task DeleteCoinAsync(Coin coin);

    IQueryable<Coin> Queryable();

    Task<Coin> GetCoinByNameAsync(string name);
}
