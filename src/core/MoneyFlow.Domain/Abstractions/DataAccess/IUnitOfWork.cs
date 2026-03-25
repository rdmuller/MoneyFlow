namespace MoneyFlow.Domain.Abstractions.DataAccess;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
