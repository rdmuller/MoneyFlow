using MoneyFlow.Domain.Common.Entities.Markets;

namespace MoneyFlow.Domain.Common.Repositories.Markets;

public interface IMarketWriteRepository
{
    Task CreateMarketAsync(Market market, CancellationToken cancellationToken = default);

    void UpdateMarket(Market market, CancellationToken cancellationToken = default);

    Task<Market> GetMarketByIdAsync(long marketId, CancellationToken cancellationToken = default);
}