using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Categories;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Domain.General.Repositories.Categories;
using MoneyFlow.Infra.DataAccess.Extensions;
using SharedKernel.Communications;

namespace MoneyFlow.Infra.DataAccess.Repositories;

public class CategoryRepository(ApplicationDbContext dbContext) : ICategoryReadRepository, ICategoryWriteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _dbContext.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<BaseQueryResponse<IEnumerable<Category>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default)
    {
        System.Linq.Expressions.Expression<Func<Category, Category>> selectorFields = m => new Category(m.Id, m.Name, m.Active, m.ExternalId);
        var query = _dbContext.Categories.AsNoTracking().AsQueryable();

        var querySpecifications = new QuerySpecification<Category>(queryParams ?? new QueryParams());

        return await querySpecifications.ExecuteQueryAsync(query, selectorFields: selectorFields, cancellationToken: cancellationToken);
    }

    public async Task<Category?> GetById(long id, CancellationToken cancellationToken = default) => await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    async Task<Category?> ICategoryReadRepository.GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Update(Category category, CancellationToken cancellationToken = default)
    {
        _dbContext.Categories.Update(category);
    }

    Task<Category?> ICategoryWriteRepository.GetByIdAsync(long categoryId, CancellationToken cancellationToken)
    {
        return _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
    }
}