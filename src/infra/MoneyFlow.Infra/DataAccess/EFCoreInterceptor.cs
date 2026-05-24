using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Application.Messaging;

namespace MoneyFlow.Infra.DataAccess;

public class EFCoreInterceptor(IDateTimeProvider timeProvider, IDomainEventsDispatcher domainEvents) : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _timeProvider = timeProvider;
    private readonly IDomainEventsDispatcher _domainEvents;

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            await PublishDomainEventsAsync(eventData.Context, cancellationToken);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        IEnumerable<IDomainEvent> domainEvents = context.ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .SelectMany(e => e.GetDomainEvents());

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await _domainEvents.DispatchAsync([domainEvent], cancellationToken);
        }
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntity(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntity(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        HandleException(eventData.Exception);

        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private static void HandleException(Exception exception)
    {
        if (exception.IsUniqueConstraintViolation())
        {
            throw DataBaseException.DuplicatedUniqueKey("A record with the same unique key already exists.");
        }
    }

    private void UpdateEntity(DbContext? context)
    {
        if (context is null)
            return;

        DateTimeOffset utcNow = _timeProvider.UtcNow;

        foreach (EntityEntry<BaseEntity> entry in context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)))
        {

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = utcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedDate = utcNow;
                    break;

                case EntityState.Deleted:
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedOn = utcNow;
                    entry.State = EntityState.Modified;
                    break;
            }
        }

        return;
    }
}
