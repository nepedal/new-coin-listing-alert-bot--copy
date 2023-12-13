namespace CryptoCoinsParser.Application;

public static class ApplicationModule
{
    public static void AddTelegramBotModule(this IServiceCollection services, IConfiguration configuration)
    {
        var botConfig = configuration.GetSection(BotConfiguration.Section).Get<BotConfiguration>();

        var defaultUserPagination = configuration.GetSection(DefaultPagination.Section).Get<DefaultPagination>();

        if (botConfig is null)
        {
            throw new ConfigurationErrorsException($"{BotConfiguration.Section} was not found");
        }

        services.AddSingleton(botConfig);
        services.AddMediatr();
        services.InjectCommandHandlers();
        services.InjectHostedServices();
        services.InjectRepositories();
        services.InjectServices();
        services.AddMemoryCache();
        services.AddSingleton(defaultUserPagination);

        var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError().
            Or<TimeoutRejectedException>() // thrown by Polly's TimeoutPolicy if the timeout is reached
            .
            Or<IOException>(e => e.Message.Contains("The response ended prematurely")).
            WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));

        services.AddHttpClient("tgwebhook").
            AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.Token, httpClient)).
            AddPolicyHandler(retryPolicy).
            AddPolicyHandler(timeoutPolicy);

        services.AddTransient<ServiceResolver<IBotCommandHandler>>(serviceProvider => key =>
        {
            if (key is null or DoNothingCallback)
            {
                return null;
            }

            return serviceProvider.ResolveCommandHandler(key);
        });

        InjectDecorators(services);
    }

    private static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(typeof(ApplicationModule).Assembly); });

        var validators = AssemblyScanner.FindValidatorsInAssembly(typeof(ApplicationModule).Assembly);

        foreach (var validator in validators)
        {
            services.AddScoped(validator.InterfaceType, validator.ValidatorType);
        }
    }

    private static void InjectServices(this IServiceCollection services)
    {
        services.AddTransient<ITelegramBotRequestLimiterService, TelegramBotRequestLimiterService>();
        services.AddSingleton<ITemplateRendererService, TemplateRendererService>();
        services.AddSingleton<ISharedTemplateRendererService, SharedTemplateRendererService>();
        services.AddTransient<ISynchronizeAnnouncementService, SynchronizeAnnouncementService>();
        services.AddTransient<IChartService, ChartService>();
        services.AddTransient<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<MenuService>();
    }

    private static void InjectRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
    }

    // [Conditional("RELEASE")]
    private static void InjectHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<NotifyAnnouncementHostedService>();
    }

    private static void InjectDecorators(IServiceCollection services)
    {
        services.Decorate<ICommandDispatcher, CommandDispatcherDecorator>();
        services.Decorate<ITelegramBotClient, TelegramBotDecorator>();
    }

    private static void InjectCommandHandlers(this IServiceCollection services)
    {
        services.AddTransient<GoToMenuCommandHandler>();
        services.AddTransient<StartBotCommandHandler>();
        services.AddTransient<DeleteMeCommandHandler>();
    }
}
