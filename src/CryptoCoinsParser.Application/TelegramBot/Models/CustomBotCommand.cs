using BotCommand = Telegram.Bot.Types.BotCommand;

namespace CryptoCoinsParser.Application.TelegramBot.Models;

public sealed class CustomBotCommand : BotCommand
{
    public Type Handler { get; set; } = default!;
}
