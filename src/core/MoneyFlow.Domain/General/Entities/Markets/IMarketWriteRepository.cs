namespace MoneyFlow.Domain.General.Entities.Markets;

public interface IMarketWriteRepository
{
    Task CreateAsync(Market market, CancellationToken cancellationToken = default);

    void Update(Market market, CancellationToken cancellationToken = default);

    Task<Market?> GetByIdAsync(long marketId, CancellationToken cancellationToken = default);

    Task<Market?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);
}