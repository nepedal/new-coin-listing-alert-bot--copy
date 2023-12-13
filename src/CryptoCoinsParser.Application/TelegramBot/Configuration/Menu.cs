namespace CryptoCoinsParser.Application.TelegramBot.Configuration;

public static class Menu
{
    public static IEnumerable<CustomBotCommand> GetMainMenuCommands()
    {
        return new[]
        {
            GoToMenuCallback
        };
    }
}
