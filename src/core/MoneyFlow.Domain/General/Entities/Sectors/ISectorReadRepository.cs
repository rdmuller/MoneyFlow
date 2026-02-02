using SharedKernel.Communications;

namespace MoneyFlow.Domain.General.Entities.Sectors;

public interface ISectorReadRepository
{
    Task<Sector> GetByExternalIdAsync(Guid externalId);

    Task<Sector> GetByIdAsync(long id);

    Task<BaseQueryResponse<IEnumerable<Sector>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default);
}
