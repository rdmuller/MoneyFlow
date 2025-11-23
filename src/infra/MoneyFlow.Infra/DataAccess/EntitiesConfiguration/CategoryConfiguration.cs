using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.Tenant.Entities.Assets;

namespace MoneyFlow.Infra.DataAccess.EntitiesConfiguration;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasAlternateKey(a => a.ExternalId);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(256);

        builder.HasIndex(a => a.Name).HasDatabaseName("ICategory2");

        builder.HasIndex(a => a.Name)
            .IsUnique()
            .HasFilter(sql: $"{nameof(Category.Active)} = 1")
            .HasDatabaseName("ICategory3");
    }
}
