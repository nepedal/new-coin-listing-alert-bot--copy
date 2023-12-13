namespace CryptoCoinsParser.Application.TelegramBot.Services.Interfaces;

public interface ISharedTemplateRendererService
{
    string RenderTopicQuestionsTemplate(string topicName, string topicDescription);
}
