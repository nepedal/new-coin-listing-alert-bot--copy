namespace CryptoCoinsParser.Persistence.EntityConfigurations;

internal sealed class AnnouncementConfiguration : BaseEntityConfiguration<Announcement, long>
{
    public override void Configure(EntityTypeBuilder<Announcement> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Message).HasColumnName(AnnouncementSchema.Columns.Message);

        builder.Property(x => x.Exchange).HasConversion<string>();

        builder.HasMany(x => x.Coins).WithMany(x => x.Announcements).UsingEntity(AnnouncementsCoinsSchema.Table);

        builder.ToTable(AnnouncementSchema.Table);
    }
}
