namespace CryptoCoinsParser.Persistence.Context;

public sealed class TelegramBotContext : DbContext
{
    public TelegramBotContext(DbContextOptions<TelegramBotContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
