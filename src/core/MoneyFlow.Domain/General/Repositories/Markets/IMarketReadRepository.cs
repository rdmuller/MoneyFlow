using MoneyFlow.Domain.General.Entities.Markets;
using SharedKernel.Communications;

namespace MoneyFlow.Domain.General.Repositories.Markets;

public interface IMarketReadRepository
{
    Task<BaseQueryResponse<IEnumerable<Market>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default);

    Task<Market?> GetByIdAsync(long marketId, CancellationToken cancellationToken = default);
}
