using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Categories;

namespace MoneyFlow.Infra.DataAccess.Repositories;
public class CategoryRepository(ApplicationDbContext dbContext) : ICategoryQueryRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Category?> GetById(long id, CancellationToken cancellationToken = default) => await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
}