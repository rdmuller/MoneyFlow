using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFlow.Domain.General.Entities.Sectors;

namespace MoneyFlow.Infra.DataAccess.EntitiesConfiguration;

internal class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasAlternateKey(a => a.ExternalId);

        builder.HasIndex(a => a.Name).HasDatabaseName("Isector2");

        builder.Property(a => a.Name).IsRequired().HasMaxLength(256);

        builder.HasOne(a => a.Category)
               .WithMany()
               .HasForeignKey(a => a.CategoryId)
               .OnDelete(DeleteBehavior.ClientCascade);
    }
}
