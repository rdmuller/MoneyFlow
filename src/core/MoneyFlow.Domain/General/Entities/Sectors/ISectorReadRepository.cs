using SharedKernel.Communications;

namespace MoneyFlow.Domain.General.Entities.Sectors;

public interface ISectorReadRepository
{
    Task<Sector?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);

    Task<Sector?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<BaseQueryResponse<IEnumerable<Sector>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default);
}
