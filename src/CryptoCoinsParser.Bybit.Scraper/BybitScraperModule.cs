namespace CryptoCoinsParser.Bybit.Scraper;

public static class BybitScraperModule
{
    public static void AddBybitScraperModule([NotNull] this IServiceCollection services,
        [NotNull] IConfiguration configuration)
    {
        services.AddTransient<BybitScraperService>();
        services.AddTransient<BybitApiService>();
    }
}
