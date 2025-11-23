using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Communications;

namespace MoneyFlow.Domain.General.Categories;
public interface ICategoryReadRepository
{
    Task<BaseQueryResponse<IEnumerable<Category>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default);

    Task<Category?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
