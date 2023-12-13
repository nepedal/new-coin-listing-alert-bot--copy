namespace CryptoCoinsParser.Application.TelegramBot.CommandHandlers.Settings;

public sealed class GoToMenuCommandHandler : IBotCommandHandler
{
    public Task Handle(Update update)
    {
        // var markup = _menuService.GetMainMenuButtons();
        //
        // await _botClient.SendTextMessageAsync(message.Chat, HelpFindPartner, replyMarkup: markup);

        return Task.CompletedTask;
    }
}
