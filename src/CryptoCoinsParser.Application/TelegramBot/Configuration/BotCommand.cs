namespace CryptoCoinsParser.Application.TelegramBot.Configuration;

public static class BotCommand
{
    public const string Pagination = "P";

    public const string DoNothingCallback = " ";

    public static readonly CustomBotCommand GoToMenuCallback = new()
    {
        Command = "menu",
        Handler = typeof(GoToMenuCommandHandler),
        Description = ButtonTextGoToMenu
    };

    public static readonly CustomBotCommand Start = new()
    {
        Command = "start",
        Handler = typeof(StartBotCommandHandler)
    };

    public static readonly CustomBotCommand DeleteMe = new()
    {
        Command = "deleteme",
        Handler = typeof(DeleteMeCommandHandler)
    };

    public static readonly CustomBotCommand CancelOperationCallback = new() //
    {
        Command = "canceloperation",
        Handler = typeof(CancelOperationCommandHandler)
    };
}
