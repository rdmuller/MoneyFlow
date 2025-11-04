using MoneyFlow.Domain.General.Entities.Categories;

namespace MoneyFlow.Domain.Common.Repositories.Categories;
public interface ICategoryQueryRepository
{
    Task<Category?> GetById(long id, CancellationToken cancellationToken = default);
}
