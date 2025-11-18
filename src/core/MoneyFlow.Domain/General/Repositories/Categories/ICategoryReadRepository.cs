using MoneyFlow.Domain.General.Entities.Categories;

namespace MoneyFlow.Domain.General.Categories;
public interface ICategoryReadRepository
{
    Task<Category?> GetById(long id, CancellationToken cancellationToken = default);
}
