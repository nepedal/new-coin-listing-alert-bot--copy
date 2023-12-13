namespace CryptoCoinsParser.Application.TelegramBot.Services.Implementation;

[UsedImplicitly]
internal class TemplateRendererService : ITemplateRendererService
{
    private readonly ILogger<TemplateRendererService> _logger;

    private readonly IMemoryCache _memoryCache;

    public TemplateRendererService(
        ILogger<TemplateRendererService> logger,
        IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public string Render(string templateName, string html, object model)
    {
        var cached = _memoryCache.Get<Template>(templateName);

        if (cached is not null && !Debugger.IsAttached)
        {
            _logger.LogDebug("Using cached template: {Name}", templateName);

            return cached.Render(model);
        }

        var result = Template.Parse(html);

        _memoryCache.Set(templateName, result);

        _logger.LogDebug("Compiled cached template: {Name}", templateName);

        return result.Render(model);
    }
}
