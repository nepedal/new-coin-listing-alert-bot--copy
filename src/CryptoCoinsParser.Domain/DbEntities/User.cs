namespace CryptoCoinsParser.Domain.DbEntities;

public record User : BaseEntity<long>
{
    public string UserName { get; set; }

    public string TelegramUserName { get; set; }

    public long TelegramUserId { get; set; }

    public string TimeZoneId { get; set; }
}
