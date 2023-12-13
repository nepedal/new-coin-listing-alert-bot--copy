namespace CryptoCoinsParser.Persistence.EntityConfigurations;

internal sealed class UserConfiguration : BaseEntityConfiguration<User, long>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.UserName).HasColumnName(UserSchema.Columns.UserName);

        builder.Property(x => x.TimeZoneId).HasColumnName(UserSchema.Columns.TimeZoneId);

        builder.Property(x => x.TelegramUserName).HasColumnName(UserSchema.Columns.TelegramUserName);

        builder.Property(x => x.TelegramUserId).HasColumnName(UserSchema.Columns.TelegramUserId);

        builder.ToTable(UserSchema.Table);
    }
}
