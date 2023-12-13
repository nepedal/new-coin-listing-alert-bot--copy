namespace CryptoCoinsParser.Persistence;

[UsedImplicitly]
internal sealed class DesignTimeContextFactory : IDesignTimeDbContextFactory<TelegramBotContext>
{
    public TelegramBotContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TelegramBotContext>();

        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).
            AddJsonFile("appsettings.json").
            AddEnvironmentVariables().
            Build();

        var connectionString = configuration.GetValue<string>(nameof(ApplicationConfiguration.DbConnectionString));

        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseExceptionProcessor();

        return new TelegramBotContext(optionsBuilder.Options);
    }
}
