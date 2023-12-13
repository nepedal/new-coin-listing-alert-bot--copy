namespace CryptoCoinsParser.Application.TelegramBot.Extensions;

public static class UpdateExtensions
{
    public static Chat GetUpdateChat(this Update update)
    {
        return update.Message?.Chat ?? update?.CallbackQuery?.Message?.Chat;
    }

    public static long GetTelegramUserId(this Update update)
    {
        return GetUpdateChat(update).Id;
    }

    public static long? GetTelegramUserIdNullable(this Update update)
    {
        return GetUpdateChat(update)?.Id;
    }

    public static long? GetDeletedTelegramUserIdNullable(this Update update)
    {
        return update?.MyChatMember?.NewChatMember?.User?.Id;
    }

    public static string GetCommand(this Update update)
    {
        return update.Message?.Text ?? update.CallbackQuery?.Data ?? update.Message?.Caption;
    }

    public static int GetMessageId(this Update update)
    {
        var messageId = update.Message?.MessageId ?? update.CallbackQuery?.Message?.MessageId;

        ArgumentNullException.ThrowIfNull(messageId);

        return messageId.Value;
    }

    public static Message GetMessage(this Update update)
    {
        var message = update.Message ?? update.CallbackQuery?.Message;

        ArgumentNullException.ThrowIfNull(message);

        return message;
    }
}
