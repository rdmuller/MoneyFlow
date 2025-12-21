using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFlow.Domain.General.Entities.Currencies;

namespace MoneyFlow.Infra.DataAccess.EntitiesConfiguration;

internal class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasAlternateKey(a => a.ExternalId);

        builder.HasIndex(a => a.Name).HasDatabaseName("icurrency2");

        builder.Property(a => a.Name).IsRequired().HasMaxLength(256);
    }
}
