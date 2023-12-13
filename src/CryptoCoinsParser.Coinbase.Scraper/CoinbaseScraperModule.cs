using CryptoCoinsParser.Coinbase.Scraper.Services;

namespace CryptoCoinsParser.Coinbase.Scraper;

public static class CoinbaseScraperModule
{
    public static void AddCoinbaseScraperModule([NotNull] this IServiceCollection services,
        [NotNull] IConfiguration configuration)
    {
        services.AddTransient<CoinbaseApiService>();
        services.AddTransient<CoinbaseScraperService>();
    }
}
