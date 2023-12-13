namespace CryptoCoinsParser.Persistence.Repositories.Implementation;

public class CoinRepository : ICoinRepository
{
    private readonly TelegramBotContext _context;

    public CoinRepository(TelegramBotContext context)
    {
        _context = context;
    }

    public Task<Coin> GetCoinAsync(long id)
    {
        return Queryable().Include(x => x.Announcements).FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Coin> GetCoinByNameAsync(string name)
    {
        return _context.Set<Coin>().Include(x => x.Announcements).FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task CreateCoinAsync(Coin coin)
    {
        _context.Set<Coin>().Add(coin);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCoinAsync(Coin coin)
    {
        _context.Set<Coin>().Remove(coin);
        await _context.SaveChangesAsync();
    }

    public IQueryable<Coin> Queryable()
    {
        return _context.Set<Coin>().AsNoTracking();
    }
}
