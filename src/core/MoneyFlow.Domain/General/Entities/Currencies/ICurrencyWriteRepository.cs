namespace MoneyFlow.Domain.General.Entities.Currencies;

public interface ICurrencyWriteRepository
{
    Task CreateAsync(Currency currency, CancellationToken cancellationToken = default);

    void Update(Currency currency, CancellationToken cancellationToken = default);

    void Delete(Currency currency, CancellationToken cancellationToken = default);

    Task<Currency?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Currency?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);

}