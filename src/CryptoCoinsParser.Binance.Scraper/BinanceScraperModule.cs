using System.Diagnostics.CodeAnalysis;
using CryptoCoinsParser.Binance.Scraper.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoCoinsParser.Binance.Scraper;

public static class BinanceScraperModule
{
    public static void AddBinanceScraperModule([NotNull] this IServiceCollection services,
        [NotNull] IConfiguration configuration)
    {
        services.AddTransient<BinanceScraperService>();
        services.AddTransient<BinanceApiService>();
    }
}
