using SharedKernel.Communications;

namespace MoneyFlow.Domain.General.Entities.Categories;

public interface ICategoryReadRepository
{
    Task<BaseQueryResponse<IEnumerable<Category>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default);

    Task<Category?> GetByIdAsync(long id, CancellationToken cancellationToken);

    Task<Category?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken);
}
