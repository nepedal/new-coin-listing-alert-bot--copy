namespace CryptoCoinsParser.Persistence.EntityConfigurations;

internal sealed class CoinConfiguration : BaseEntityConfiguration<Coin, long>
{
    public override void Configure(EntityTypeBuilder<Coin> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).HasColumnName(CoinSchema.Columns.Name);

        builder.HasMany(x => x.Announcements).WithMany(x => x.Coins).UsingEntity(AnnouncementsCoinsSchema.Table);

        builder.ToTable(CoinSchema.Table);
    }
}
