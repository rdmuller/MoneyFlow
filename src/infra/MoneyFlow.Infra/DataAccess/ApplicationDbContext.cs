using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.Entities;

namespace MoneyFlow.Infra.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) {}

    DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DbSchemas.APPLICATION);

        base.OnModelCreating(modelBuilder);
    }
}