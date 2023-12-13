namespace CryptoCoinsParser.Application.TelegramBot.Services.Implementation;

internal sealed class SharedTemplateRendererService : ISharedTemplateRendererService
{
    private readonly ITemplateRendererService _templateRendererService;

    public SharedTemplateRendererService(ITemplateRendererService templateRendererService)
    {
        _templateRendererService = templateRendererService;
    }

    public string RenderTopicQuestionsTemplate(string topicName, string topicDescription)
    {
        var topicQuestionsTemplateName = "TopicQuestions".ToLocalizedTemplateId();

        return _templateRendererService.Render(topicQuestionsTemplateName,
            QuestionsTemplate, new { topicName, topicDescription });
    }
}
