namespace CryptoCoinsParser.Domain.DbEntities;

public abstract record BaseEntity<TId>
{
    public TId Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
