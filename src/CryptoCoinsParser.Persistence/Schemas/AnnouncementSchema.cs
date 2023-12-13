namespace CryptoCoinsParser.Persistence.Schemas;

public static class AnnouncementSchema
{
    public const string Table = "announcements";

    [UsedImplicitly]
    public class Columns : ColumnsBase
    {
        public const string Message = "message";
    }
}
