namespace Shared;

public static class SharedModule
{
    public static void AddSharedModule(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationConfiguration = configuration.Get<ApplicationConfiguration>();

        if (applicationConfiguration is null)
        {
            throw new ArgumentNullException(nameof(ApplicationConfiguration));
        }

        services.AddSingleton(applicationConfiguration);
    }
}
