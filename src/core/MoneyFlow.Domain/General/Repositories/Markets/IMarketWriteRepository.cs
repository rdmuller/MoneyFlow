using MoneyFlow.Domain.General.Entities.Markets;

namespace MoneyFlow.Domain.General.Repositories.Markets;

public interface IMarketWriteRepository
{
    Task CreateMarketAsync(Market market, CancellationToken cancellationToken = default);

    void UpdateMarket(Market market, CancellationToken cancellationToken = default);

    Task<Market?> GetMarketByIdAsync(long marketId, CancellationToken cancellationToken = default);
}