namespace CryptoCoinsParser.KuCoin.Scraper;

public static class KuCoinScraperModule
{
    public static void AddKuCoinScraperModule([NotNull] this IServiceCollection services,
        [NotNull] IConfiguration configuration)
    {
        services.AddTransient<KuCoinScraperService>();
        services.AddTransient<KuCoinApiService>();
    }
}
