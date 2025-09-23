using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFlow.Domain.Common.Entities.Categories;

namespace MoneyFlow.Infra.DataAccess.EntitiesConfiguration;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasAlternateKey(a => a.ExternalId);

        builder.HasIndex(a => a.Name).HasDatabaseName("ICategory2");

        builder.Property(a => a.Name).IsRequired().HasMaxLength(256);
    }
}
