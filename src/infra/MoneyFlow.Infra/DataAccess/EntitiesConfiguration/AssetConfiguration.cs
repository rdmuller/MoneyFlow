using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFlow.Domain.Tenant.Entities.Assets;

namespace MoneyFlow.Infra.DataAccess.EntitiesConfiguration;

internal class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasAlternateKey(a => a.ExternalId);

        builder.HasIndex(a => new { a.TenantId, a.Ticker })
            .IsUnique()
            .HasFilter(sql: $"{nameof(Asset.Ticker)} is not null and {nameof(Asset.Ticker)} <> ''")
            .HasDatabaseName("iasset1");
        builder.HasIndex(a => new { a.TenantId, a.Name }).HasDatabaseName("iasset2");
        builder.HasIndex(a => new { a.TenantId, a.CategoryId }).HasDatabaseName("iasset3");

        builder.Property(a => a.Name).IsRequired().HasMaxLength(256);
        builder.Property(a => a.Ticker).IsRequired().HasMaxLength(50);

        builder.HasOne(a => a.Category)
               .WithMany()
               .HasForeignKey(a => a.CategoryId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.Sector)
                .WithMany()
                .HasForeignKey(a => a.SectorId)
                .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.Wallet)
                .WithMany()
                .HasForeignKey(a => a.WalletId)
                .OnDelete(DeleteBehavior.NoAction);

        builder.ToTable(tb => tb.HasCheckConstraint(
            "ck_ticker_with_empty_space",
            sql: $"rtrim({nameof(Asset.Ticker)})={nameof(Asset.Ticker)}"));
    }
}
