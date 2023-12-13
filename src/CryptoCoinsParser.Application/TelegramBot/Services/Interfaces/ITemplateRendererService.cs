namespace CryptoCoinsParser.Application.TelegramBot.Services.Interfaces;

public interface ITemplateRendererService
{
    string Render(string templateName, string html, object model);
}
