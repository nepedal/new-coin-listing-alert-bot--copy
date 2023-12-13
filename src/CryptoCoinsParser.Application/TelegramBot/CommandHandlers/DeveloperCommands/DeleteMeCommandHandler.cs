namespace CryptoCoinsParser.Application.TelegramBot.CommandHandlers.DeveloperCommands;

public sealed class DeleteMeCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;

    private readonly IUserRepository _userRepository;

    public DeleteMeCommandHandler(ITelegramBotClient botClient, IUserRepository userRepository)
    {
        _botClient = botClient;
        _userRepository = userRepository;
    }

    public async Task Handle(Update update)
    {
        var telegramUserId = update.GetTelegramUserId();

        await _userRepository.DeleteUserByTelegramUserIdAsync(telegramUserId);

        await _botClient.SendTextMessageAsync(telegramUserId, "User deleted");
    }
}
