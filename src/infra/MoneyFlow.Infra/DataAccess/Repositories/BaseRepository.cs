using SharedKernel.Entities;

namespace MoneyFlow.Infra.DataAccess.Repositories;

abstract internal class BaseRepository<T>(ApplicationDbContext dbContext) where T : BaseEntity
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    }
}
