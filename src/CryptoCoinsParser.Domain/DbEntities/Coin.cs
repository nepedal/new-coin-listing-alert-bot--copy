namespace CryptoCoinsParser.Domain.DbEntities;

public record Coin : BaseEntity<long>
{
    public string Name { get; set; }

    public List<Announcement> Announcements { get; set; }
}
