using MoneyFlow.Domain.Abstractions.DataAccess;

namespace MoneyFlow.Infra.DataAccess;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task CommitAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);
}