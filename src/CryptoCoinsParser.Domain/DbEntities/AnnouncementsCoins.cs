namespace CryptoCoinsParser.Domain.DbEntities;

public record AnnouncementsCoins
{
    public Coin Coin { get; set; }

    public Announcement Announcement { get; set; }
}
