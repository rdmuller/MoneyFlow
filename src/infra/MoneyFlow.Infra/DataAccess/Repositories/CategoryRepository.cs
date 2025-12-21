using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Categories;
using MoneyFlow.Domain.General.Entities.Categories;
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
        var query = _dbContext.Categories.AsNoTracking().Select(a => new Category
        {
            Id = a.Id,
            Name = a.Name,
            Active = a.Active,
            ExternalId = null
        }).AsQueryable();

        var querySpecifications = new QuerySpecification<Category>(queryParams ?? new QueryParams());

        return await querySpecifications.ExecuteQueryAsync(query, cancellationToken: cancellationToken);
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