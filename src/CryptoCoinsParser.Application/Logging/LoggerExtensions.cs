namespace CryptoCoinsParser.Application.Logging;

public static class LoggerExtensions
{
    public static void AddTelegramLogger(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var telegramLoggerConfiguration = configuration.GetSection("Logging").
            GetSection(TelegramLoggerConfiguration.Section).
            Get<TelegramLoggerConfiguration>();

        if (telegramLoggerConfiguration.Enabled)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TelegramLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions<TelegramLoggerConfiguration, TelegramLogger>(builder.Services);

            builder.Services.AddSingleton(telegramLoggerConfiguration);
        }
    }
}
