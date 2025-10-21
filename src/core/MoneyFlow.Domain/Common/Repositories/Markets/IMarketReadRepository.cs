using MoneyFlow.Domain.Common.Entities.Markets;

namespace MoneyFlow.Domain.Common.Repositories.Markets;

public interface IMarketReadRepository
{
    Task<IEnumerable<Market>> GetAllMarketsAsync(CancellationToken cancellationToken = default);

    Task<Market?> GetMarketByIdAsync(long marketId, CancellationToken cancellationToken = default);
}
