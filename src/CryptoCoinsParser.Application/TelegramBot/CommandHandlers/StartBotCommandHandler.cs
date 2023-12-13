using Microsoft.EntityFrameworkCore;
using User = CryptoCoinsParser.Domain.DbEntities.User;

namespace CryptoCoinsParser.Application.TelegramBot.CommandHandlers;

public sealed class StartBotCommandHandler : IBotCommandHandler
{
    private readonly ITelegramBotClient _botClient;

    private readonly IUserRepository _userRepository;

    public StartBotCommandHandler(
        ITelegramBotClient botClient,
        IUserRepository userRepository)
    {
        _botClient = botClient;
        _userRepository = userRepository;
    }

    public async Task Handle(Update update)
    {
        var message = update.GetMessage();
        var chat = message.Chat;

        var userExists = await _userRepository.Queryable().AnyAsync(x => x.TelegramUserId == chat.Id);

        if (userExists)
        {
            await _botClient.SendTextMessageAsync(message.Chat, AlreadyRegistered);
        }
        else
        {
            var user = new User
            {
                TelegramUserId = message.Chat.Id,
                TelegramUserName = chat.Username,
                UserName = $"{chat.FirstName} {chat.LastName}".Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateUserAsync(user);

            await _botClient.SendTextMessageAsync(message.Chat, InitialMessage);
        }
    }
}
