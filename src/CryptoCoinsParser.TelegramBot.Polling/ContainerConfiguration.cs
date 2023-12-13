using CryptoCoinsParser.Coinbase.Scraper;

namespace CryptoCoinsParser.TelegramBot.Polling;

public static class ContainerConfiguration
{
    public static IServiceProvider Configure()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        services.AddOptions();

        services.AddSharedModule(configuration);

        services.AddPersistenceModule(configuration);

        services.AddBinanceScraperModule(configuration);

        services.AddCoinbaseScraperModule(configuration);

        services.AddOkxScraperModule(configuration);

        services.AddBybitScraperModule(configuration);

        services.AddKuCoinScraperModule(configuration);

        services.AddTelegramBotModule(configuration);

        services.AddSingleton<IConfiguration>(configuration);

        return services.BuildServiceProvider();
    }
}
