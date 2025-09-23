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

        builder.HasIndex(a => new { a.TenantId, a.Ticker }).IsUnique().HasFilter("Code is not null and RTRIM(Code) <> ''").HasDatabaseName("IActive1");
        builder.HasIndex(a => new { a.TenantId, a.Name }).HasDatabaseName("IAsset2");
        builder.HasIndex(a => new { a.TenantId, a.CategoryId }).HasDatabaseName("IAsset3");

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
    }
}
