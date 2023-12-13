using Microsoft.Extensions.Logging;
using Polly;

namespace CryptoCoinsParser.Persistence.Services;

public sealed class AutomaticDbMigrationService : IAutomaticDbMigrationService
{
    private const int Retries = 10;

    private readonly ILogger<AutomaticDbMigrationService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AutomaticDbMigrationService(ILogger<AutomaticDbMigrationService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void Run()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TelegramBotContext>>();

            var retry = Policy.Handle<Exception>().
                WaitAndRetry(10,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, __, _, ___) =>
                    {
                        logger.LogWarning(exception,
                            "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}",
                            nameof(TelegramBotContext), exception.GetType().Name, exception.Message, _, Retries);
                    });

            //if the database server container is not yet created on run docker compose this
            //migration can't fail for network related exception. The retry options for DbContext only 
            //apply to transient exceptions
            retry.Execute(() =>
            {
                var database = services.GetRequiredService<TelegramBotContext>().Database;

                database.SetCommandTimeout(160);

                database.Migrate();
            });
        }

        _logger.LogInformation("Finished running automatic migration...");
    }
}
