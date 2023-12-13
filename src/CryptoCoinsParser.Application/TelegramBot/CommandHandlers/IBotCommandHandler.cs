namespace CryptoCoinsParser.Application.TelegramBot.CommandHandlers;

public interface IBotCommandHandler
{
    Task Handle(Update update);
}
