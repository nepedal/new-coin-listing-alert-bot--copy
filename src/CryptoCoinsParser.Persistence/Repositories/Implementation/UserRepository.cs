namespace CryptoCoinsParser.Persistence.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly TelegramBotContext _context;

    public UserRepository(TelegramBotContext context)
    {
        _context = context;
    }

    public async Task CreateUserAsync(User user)
    {
        _context.Set<User>().Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.Set<User>().Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserByTelegramUserIdAsync(long telegramUserId)
    {
        var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.TelegramUserId == telegramUserId);

        if (user != null)
        {
            _context.Set<User>().Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User> GetUserAsync(long id)
    {
        return await Queryable().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> GetUserByTelegramUserIdAsync(long telegramUserId)
    {
        return await Queryable().FirstOrDefaultAsync(x => x.Id == telegramUserId);
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Set<User>().ToListAsync();
    }

    public IQueryable<User> Queryable()
    {
        return _context.Set<User>().AsNoTracking();
    }
}
