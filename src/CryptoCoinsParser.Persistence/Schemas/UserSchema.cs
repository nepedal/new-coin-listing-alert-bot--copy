namespace CryptoCoinsParser.Persistence.Schemas;

public static class UserSchema
{
    public const string Table = "users";

    [UsedImplicitly]
    public class Columns : ColumnsBase
    {
        public const string UserName = "user_name";

        public const string TimeZoneId = "time_zone_id";

        public const string TelegramUserName = "telegram_user_name";

        public const string TelegramUserId = "telegram_user_id";
    }
}
