using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Currencies;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Domain.General.Entities.Sectors;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.Tenant.Entities.Assets;
using MoneyFlow.Domain.Tenant.Entities.Wallets;

namespace MoneyFlow.Infra.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) {}

    #region Common entities
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<Market> Markets { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    #endregion

    #region Tenant entities
    public DbSet<Asset> Assets { get; set; }
    //public DbSet<Operation> Operations { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DbSchemas.APPLICATION);

        base.OnModelCreating(modelBuilder);
    }
}