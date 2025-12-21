using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFlow.Domain.Tenant.Entities.Wallets;

namespace MoneyFlow.Infra.DataAccess.EntitiesConfiguration;

internal class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasAlternateKey(a => a.ExternalId);

        builder.HasIndex(a => new { a.TenantId, a.Name }).HasDatabaseName("iwallet1");

        builder.Property(a => a.Name).IsRequired().HasMaxLength(256);
    }
}
