using CryptoCoinsParser.Coinbase.Scraper;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.WebHost.UseShutdownTimeout(TimeSpan.FromSeconds(10));
builder.WebHost.UseSentry(x =>
{
    x.IncludeActivityData = true;
    x.MaxRequestBodySize = RequestSize.Always;
});

builder.Services.AddControllers();
builder.Logging.AddTelegramLogger(builder.Configuration);

services.AddHttpContextAccessor();

services.AddCors();

AddAutomapper(services);

AddModules(builder);
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<ConfigureWebhookHostedService>();
}

#pragma warning disable CS7022
var app = builder.Build();

app.UseHttpLogging();

var options = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};

options.KnownNetworks.Clear();
options.KnownProxies.Clear();
app.UseForwardedHeaders(options);

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ContractResolver = new CamelCasePropertyNamesContractResolver()
};

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseOpenApi();
app.UseSwaggerUi3();
app.UseReDoc(settings =>
{
    settings.DocumentPath = "/redoc/index.html";
    settings.Path = "/redoc";
});

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

AddMinimalApiRoutes(app);

var lifetime = app.Lifetime;
var logger = app.Logger;

lifetime.ApplicationStarted.Register(() => logger.LogWarning("The application started..."));
lifetime.ApplicationStopped.Register(() => logger.LogWarning("The application stopped..."));

using (var scope = app.Services.CreateScope())
{
    var migrationRunner = scope.ServiceProvider.GetService<IAutomaticDbMigrationService>();
    migrationRunner?.Run();
}

app.MapControllers();
app.Run();

static void AddMinimalApiRoutes(WebApplication app)
{
    app.MapTelegramBotRoutes();
}

static void AddModules(WebApplicationBuilder builder)
{
    builder.Services.AddPersistenceModule(builder.Configuration);

    builder.Services.AddBinanceScraperModule(builder.Configuration);

    builder.Services.AddCoinbaseScraperModule(builder.Configuration);

    builder.Services.AddOkxScraperModule(builder.Configuration);

    builder.Services.AddBybitScraperModule(builder.Configuration);

    builder.Services.AddKuCoinScraperModule(builder.Configuration);

    builder.Services.AddTelegramBotModule(builder.Configuration);

    builder.Services.AddSharedModule(builder.Configuration);
}

static void AddAutomapper(IServiceCollection services)
{
    var mapperConfig = new MapperConfiguration(mc => mc.AddMaps(typeof(PersistenceModule).Assembly));

    var mapper = mapperConfig.CreateMapper();

    mapperConfig.AssertConfigurationIsValid();

    mapperConfig.EnsureAllPropertiesAreMapped();

    services.AddSingleton(mapper);
}

// to run tests using WebApplicationFactory 
public partial class Program
{
}
