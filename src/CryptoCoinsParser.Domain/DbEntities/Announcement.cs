namespace CryptoCoinsParser.Domain.DbEntities;

public record Announcement : BaseEntity<long>
{
    public string Message { get; set; }

    public ExchangeName Exchange { get; set; }

    public List<Coin> Coins { get; set; }
}
