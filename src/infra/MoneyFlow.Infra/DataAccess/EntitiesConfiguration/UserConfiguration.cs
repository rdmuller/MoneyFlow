using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFlow.Domain.Entities;

namespace MoneyFlow.Infra.DataAccess.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasAlternateKey(u => u.ExternalId);
        builder.HasIndex(u => u.Email).HasDatabaseName("iuser1");

        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.Name).HasMaxLength(256);
        builder.Property(u => u.Password).HasMaxLength(100);
        builder.Property(u => u.Role).HasMaxLength(20).IsRequired();
    }
}