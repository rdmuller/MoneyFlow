using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.Abstractions.Events;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Currencies;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Domain.General.Entities.Sectors;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.Tenant.Entities.Assets;
using MoneyFlow.Domain.Tenant.Entities.Wallets;
using MoneyFlow.Domain.Tenant.Services;
using SharedKernel.Abstractions;
using SharedKernel.Entities;
using System.Data;

namespace MoneyFlow.Infra.DataAccess;

public class ApplicationDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;
    private readonly IDomainEventsDispatcher _domainEvents;

    public ApplicationDbContext(
        DbContextOptions options, 
        ITenantProvider tenantProvider,
        IDomainEventsDispatcher domainEvents) : base(options)
    {
        _tenantProvider = tenantProvider;
        _domainEvents = domainEvents;
    }

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEventsAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new DBConcurrencyException("Concurrency exception ocurred.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while saving changes to the database.", ex);
        }
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .SelectMany(e => e.GetDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _domainEvents.DispatchAsync([domainEvent], cancellationToken);
        }
    }
}