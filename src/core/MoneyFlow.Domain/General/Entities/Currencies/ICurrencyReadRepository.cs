using SharedKernel.Communications;

namespace MoneyFlow.Domain.General.Entities.Currencies;

public interface ICurrencyReadRepository
{
    Task<Currency?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Currency?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);

    Task<BaseQueryResponse<IEnumerable<Currency>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default);
}
