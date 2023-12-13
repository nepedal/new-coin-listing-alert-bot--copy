namespace CryptoCoinsParser.Okx.Scraper;

public static class OkxScraperModule
{
    public static void AddOkxScraperModule([NotNull] this IServiceCollection services,
        [NotNull] IConfiguration configuration)
    {
        services.AddTransient<OkxScraperService>();
        services.AddTransient<OkxApiService>();
    }
}
