using MoneyFlow.Domain.General.Entities.Markets;

namespace MoneyFlow.Domain.General.Repositories.Markets;

public interface IMarketReadRepository
{
    Task<IEnumerable<Market>> GetAllMarketsAsync(CancellationToken cancellationToken = default);

    Task<Market?> GetMarketByIdAsync(long marketId, CancellationToken cancellationToken = default);
}
