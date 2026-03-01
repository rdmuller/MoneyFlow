using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Currencies;
using MoneyFlow.Infra.DataAccess.Extensions;
using SharedKernel.Communications;
using System.Linq.Expressions;

namespace MoneyFlow.Infra.DataAccess.Repositories;

internal class CurrencyRepository : BaseRepository<Currency>, ICurrencyWriteRepository, ICurrencyReadRepository
{
    public CurrencyRepository(ApplicationDbContext dbContext) : base(dbContext) {}

    public async Task<BaseQueryResponse<IEnumerable<Currency>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default)
    {
        Expression<Func<Currency, Currency>> selectorFields = c => new Currency(c.Id, c.Name, c.Symbol, c.Active, c.ExternalId);
        var query = _dbContext.Currencies.AsNoTracking().AsQueryable();
        var querySpecification = new QuerySpecification<Currency>(queryParams ?? new QueryParams());

        return await querySpecification.ExecuteQueryAsync(query, selectorFields: selectorFields, orderBy: c => c.Name, cancellationToken: cancellationToken);
    }

    public Task<Currency?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Currency?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
