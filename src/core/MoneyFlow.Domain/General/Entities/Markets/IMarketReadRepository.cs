using Shared.Domain;

namespace MoneyFlow.Domain.General.Entities.Markets;

public interface IMarketReadRepository
{
    Task<Result<IEnumerable<Market>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default);

    Task<Market?> GetByIdAsync(long marketId, CancellationToken cancellationToken = default);

    Task<Market?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);
}
