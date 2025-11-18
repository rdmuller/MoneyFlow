using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Categories;
using MoneyFlow.Domain.General.Repositories.Categories;

namespace MoneyFlow.Infra.DataAccess.Repositories;
public class CategoryRepository(ApplicationDbContext dbContext) : ICategoryReadRepository, ICategoryWriteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _dbContext.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<Category?> GetById(long id, CancellationToken cancellationToken = default) => await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public void Update(Category category, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    Task ICategoryWriteRepository.GetById(long categoryId, CancellationToken cancellationToken)
    {
        return GetById(categoryId, cancellationToken);
    }
}