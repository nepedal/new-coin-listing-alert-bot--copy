namespace CryptoCoinsParser.Application.TelegramBot.Services.Interfaces;

public interface ICommandDispatcher
{
    Task DispatchAsync(Update update);

    Task ExecuteCommandHandler(IBotCommandHandler commandHandler, Update update);
}
