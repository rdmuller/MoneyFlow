using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Infra.DataAccess.Extensions;
using SharedKernel.Communications;

namespace MoneyFlow.Infra.DataAccess.Repositories;

internal sealed class MarketRepository : BaseRepository<Market>, IMarketReadRepository, IMarketWriteRepository
{
    public MarketRepository(ApplicationDbContext dbContext) : base(dbContext) {}

    public async Task<BaseQueryResponse<IEnumerable<Market>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default)
    {
        System.Linq.Expressions.Expression<Func<Market, Market>> selectorFields = m => new Market(m.Id, m.Name, m.Active, m.ExternalId);
        var query = _dbContext.Markets.AsNoTracking().AsQueryable();
        var querySpecification = new QuerySpecification<Market>(queryParams ?? new QueryParams());

        //query = querySpecification.ApplyFilters(query);

        //var total = await query.CountAsync(cancellationToken);

        //query = querySpecification.AddPagination(query);

        //return new BaseQueryResponse<IEnumerable<Market>>
        //{
        //    TotalRows = total,
        //    Data = await query.ToListAsync(cancellationToken)
        //};

        return await querySpecification.ExecuteQueryAsync(query, selectorFields: selectorFields, orderBy: (m => m.Name), cancellationToken: cancellationToken);
    }

    async Task<Market?> IMarketReadRepository.GetByIdAsync(long marketId, CancellationToken cancellationToken)
        => await _dbContext.Markets.AsNoTracking().FirstOrDefaultAsync(t => t.Id.Equals(marketId), cancellationToken);

    async Task<Market?> IMarketWriteRepository.GetByIdAsync(long marketId, CancellationToken cancellationToken)
        => await _dbContext.Markets.FirstOrDefaultAsync(t => t.Id.Equals(marketId), cancellationToken);

    async Task<Market?> IMarketReadRepository.GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
        => await _dbContext.Markets.AsNoTracking().FirstOrDefaultAsync(t => t.ExternalId.Equals(externalId), cancellationToken);

    async Task<Market?> IMarketWriteRepository.GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken) 
        => await _dbContext.Markets.FirstOrDefaultAsync(t => t.ExternalId.Equals(externalId), cancellationToken);

}
