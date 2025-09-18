using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.Entities.Actives;
using MoneyFlow.Domain.Entities.Investments;
using MoneyFlow.Domain.Entities.InvestmentTypes;
using MoneyFlow.Domain.Entities.Users;

namespace MoneyFlow.Infra.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Active> Actives { get; set; }
    public DbSet<ActiveType> ActiveTypes { get; set; }
    public DbSet<Investment> Investments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DbSchemas.APPLICATION);

        base.OnModelCreating(modelBuilder);
    }
}