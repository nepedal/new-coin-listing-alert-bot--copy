namespace CryptoCoinsParser.Application.TelegramBot;

public class CancelOperationCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;

    public CancelOperationCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task Handle(Update update)
    {
        var message = update.GetMessage();

        await _botClient.EditMessageTextAsync(message.Chat, message.MessageId, OperationDeclined,
            replyMarkup: InlineButtons.WithRow(GoToMenuButton).ToMarkup());
    }
}
