namespace MoneyFlow.Domain.General.Entities.Categories;

public interface ICategoryWriteRepository
{
    Task CreateAsync(Category category, CancellationToken cancellationToken = default);

    void Update(Category category, CancellationToken cancellationToken = default);

    Task<Category?> GetByIdAsync(long categoryId, CancellationToken cancellationToken = default);

    Task<Category?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);
}
