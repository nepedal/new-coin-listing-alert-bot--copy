namespace CryptoCoinsParser.Persistence;

public static class PersistenceModule
{
    public static void AddPersistenceModule([NotNull] this IServiceCollection services,
        [NotNull] IConfiguration configuration)
    {
        EntityFrameworkManager.IsCommunity = true;
        services.AddSingleton<IAutomaticDbMigrationService, AutomaticDbMigrationService>();
        var connectionString = configuration.GetValue<string>(nameof(ApplicationConfiguration.DbConnectionString));

        services.AddDbContext<TelegramBotContext>(options =>
        {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString);
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.BoolWithDefaultWarning));
        });

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IAnnouncementRepository, AnnouncementRepository>();
        services.AddTransient<ICoinRepository, CoinRepository>();

        services.AddSingleton(typeof(IKeyValueRepository<>), typeof(KeyValueRepository<>));

        services.AddDistributedMemoryCache();

        AddAutomapper(services);
    }

    private static void AddAutomapper(IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc => mc.AddMaps(typeof(PersistenceModule).Assembly));

        var mapper = mapperConfig.CreateMapper();

        mapperConfig.AssertConfigurationIsValid();

        mapperConfig.EnsureAllPropertiesAreMapped();

        services.AddSingleton(mapper);
    }
}
