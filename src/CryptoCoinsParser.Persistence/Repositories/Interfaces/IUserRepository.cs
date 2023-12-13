namespace CryptoCoinsParser.Persistence.Repositories.Interfaces;

public interface IUserRepository
{
    Task CreateUserAsync(User user);

    Task DeleteUserAsync(User user);

    Task DeleteUserByTelegramUserIdAsync(long telegramUserId);

    public Task<User> GetUserByTelegramUserIdAsync(long telegramUserId);

    Task<User> GetUserAsync(long id);

    Task<List<User>> GetUsersAsync();

    IQueryable<User> Queryable();
}
