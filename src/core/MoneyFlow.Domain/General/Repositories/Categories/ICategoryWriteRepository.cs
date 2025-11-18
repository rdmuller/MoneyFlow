using MoneyFlow.Domain.General.Entities.Categories;

namespace MoneyFlow.Domain.General.Repositories.Categories;

public interface ICategoryWriteRepository
{
    Task CreateAsync(Category category, CancellationToken cancellationToken = default);

    void Update(Category category, CancellationToken cancellationToken = default);

    Task GetById(long categoryId, CancellationToken cancellationToken = default);
}
