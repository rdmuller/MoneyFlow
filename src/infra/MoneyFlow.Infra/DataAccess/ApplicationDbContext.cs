using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Currencies;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Domain.General.Entities.Sectors;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.Tenant.Entities.Assets;
using MoneyFlow.Domain.Tenant.Entities.Wallets;
using MoneyFlow.Domain.Tenant.Services;

namespace MoneyFlow.Infra.DataAccess;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly ITenantProvider _tenantProvider;

    public ApplicationDbContext(
        DbContextOptions options,
        ITenantProvider tenantProvider) : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    #region Common entities
    internal DbSet<User> Users { get; set; }
    internal DbSet<Category> Categories { get; set; }
    internal DbSet<Sector> Sectors { get; set; }
    internal DbSet<Market> Markets { get; set; }
    internal DbSet<Currency> Currencies { get; set; }
    #endregion

    #region Tenant entities
    internal DbSet<Asset> Assets { get; set; }
    //public DbSet<Operation> Operations { get; set; }
    internal DbSet<Wallet> Wallets { get; set; }
    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DbSchemas.Application);

        modelBuilder.Entity<Category>().HasQueryFilter(DefaultFilters.SoftDeletionFilter, e => !e.IsDeleted);
        modelBuilder.Entity<Sector>().HasQueryFilter(DefaultFilters.SoftDeletionFilter, e => !e.IsDeleted);
        modelBuilder.Entity<Market>().HasQueryFilter(DefaultFilters.SoftDeletionFilter, e => !e.IsDeleted);
        modelBuilder.Entity<Currency>().HasQueryFilter(DefaultFilters.SoftDeletionFilter, e => !e.IsDeleted);

        modelBuilder.Entity<Asset>().HasQueryFilter(DefaultFilters.SoftDeletionFilter, e => !e.IsDeleted)
            .HasQueryFilter(DefaultFilters.TenantFilter, e => e.TenantId == _tenantProvider.Get());

        modelBuilder.Entity<Wallet>().HasQueryFilter(DefaultFilters.SoftDeletionFilter, e => !e.IsDeleted)
            .HasQueryFilter(DefaultFilters.TenantFilter, e => e.TenantId == _tenantProvider.Get());

        base.OnModelCreating(modelBuilder);
    }
}
