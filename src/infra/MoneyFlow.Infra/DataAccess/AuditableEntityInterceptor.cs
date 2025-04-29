using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MoneyFlow.Common.Entities;
using MoneyFlow.Common.Services;

namespace MoneyFlow.Infra.DataAccess;

public class AuditableEntityInterceptor(IDateTimeProvider timeProvider) : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _timeProvider = timeProvider;

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

    private void UpdateEntity(DbContext? context)
    {
        if (context is null)
            return;

        var utcNow = _timeProvider.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified)))
        {

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = utcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedDate = utcNow;
                    break;
            }
        }

        return;
    }
}
