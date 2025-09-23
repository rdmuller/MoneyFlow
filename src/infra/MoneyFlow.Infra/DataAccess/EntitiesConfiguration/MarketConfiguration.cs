using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFlow.Domain.Common.Entities.Markets;

namespace MoneyFlow.Infra.DataAccess.EntitiesConfiguration;

internal class MarketConfiguration : IEntityTypeConfiguration<Market>
{
    public void Configure(EntityTypeBuilder<Market> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasAlternateKey(a => a.ExternalId);

        builder.HasIndex(a => a.Name).HasDatabaseName("IMarket2");

        builder.Property(a => a.Name).IsRequired().HasMaxLength(256);
    }
}
