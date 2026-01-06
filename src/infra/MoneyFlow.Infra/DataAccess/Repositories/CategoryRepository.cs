using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Infra.DataAccess.Extensions;
using SharedKernel.Communications;

namespace MoneyFlow.Infra.DataAccess.Repositories;

internal sealed class CategoryRepository : BaseRepository<Category>, ICategoryReadRepository, ICategoryWriteRepository
{
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext) {}

    public async Task<BaseQueryResponse<IEnumerable<Category>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default)
    {
        System.Linq.Expressions.Expression<Func<Category, Category>> selectorFields = m => new Category(m.Id, m.Name, m.Active, m.ExternalId);
        var query = _dbContext.Categories.AsNoTracking().OrderBy(c => c.Name).AsQueryable();

        var querySpecifications = new QuerySpecification<Category>(queryParams ?? new QueryParams());

        return await querySpecifications.ExecuteQueryAsync(query, selectorFields: selectorFields, cancellationToken: cancellationToken);
    }

    async Task<Category?> ICategoryReadRepository.GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken) 
        => await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.ExternalId.Equals(externalId), cancellationToken);

    async Task<Category?> ICategoryWriteRepository.GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
        => await _dbContext.Categories.FirstOrDefaultAsync(c => c.ExternalId.Equals(externalId), cancellationToken);

    async Task<Category?> ICategoryReadRepository.GetByIdAsync(long id, CancellationToken cancellationToken)
        => await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    async Task<Category?> ICategoryWriteRepository.GetByIdAsync(long categoryId, CancellationToken cancellationToken)
        => await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
}