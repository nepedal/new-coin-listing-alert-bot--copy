namespace CryptoCoinsParser.Persistence.EntityConfigurations;

internal class BaseEntityConfiguration<T, TKey> : IEntityTypeConfiguration<T> where T : BaseEntity<TKey>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.Id).HasColumnName(ColumnsBase.Id);

        builder.Property(x => x.CreatedAt).HasColumnName(ColumnsBase.CreatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();

        builder.HasKey(x => x.Id);
    }
}
