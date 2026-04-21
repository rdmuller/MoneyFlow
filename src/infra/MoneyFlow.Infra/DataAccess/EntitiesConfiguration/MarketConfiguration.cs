using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFlow.Domain.General.Entities.Markets;

namespace MoneyFlow.Infra.DataAccess.EntitiesConfiguration;

internal class MarketConfiguration : IEntityTypeConfiguration<Market>
{
    public void Configure(EntityTypeBuilder<Market> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasAlternateKey(a => a.ExternalId);

        builder.HasIndex(a => a.Name)
            .IsUnique()
            .HasFilter(sql: "is_deleted = false")
            .HasDatabaseName("imarket2");

        builder.Property(a => a.Name).IsRequired().HasMaxLength(256);
    }
}
