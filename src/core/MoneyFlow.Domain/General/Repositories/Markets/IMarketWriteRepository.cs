using MoneyFlow.Domain.General.Entities.Markets;

namespace MoneyFlow.Domain.General.Repositories.Markets;

public interface IMarketWriteRepository
{
    Task CreateAsync(Market market, CancellationToken cancellationToken = default);

    void Update(Market market, CancellationToken cancellationToken = default);

    Task<Market?> GetByIdAsync(long marketId, CancellationToken cancellationToken = default);
}