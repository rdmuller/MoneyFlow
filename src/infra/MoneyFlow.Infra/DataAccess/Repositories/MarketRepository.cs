using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Domain.General.Repositories.Markets;
using MoneyFlow.Infra.DataAccess.Extensions;
using SharedKernel.Communications;

namespace MoneyFlow.Infra.DataAccess.Repositories;

public class MarketRepository(ApplicationDbContext dbContext) : IMarketReadRepository, IMarketWriteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task CreateAsync(Market market, CancellationToken cancellationToken = default)
    {
        await _dbContext.Markets.AddAsync(market, cancellationToken);
    }

    public async Task<BaseQueryResponse<IEnumerable<Market>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Markets.AsNoTracking().Select(m => new Market(m.Id, m.Name, m.Active, m.ExternalId)).AsQueryable();
        //var query = _dbContext.Markets.AsNoTracking().AsQueryable();
        var querySpecification = new QuerySpecification<Market>(queryParams ?? new QueryParams());

        //query = querySpecification.ApplyFilters(query);

        //var total = await query.CountAsync(cancellationToken);

        //query = querySpecification.AddPagination(query);

        //return new BaseQueryResponse<IEnumerable<Market>>
        //{
        //    TotalRows = total,
        //    Data = await query.ToListAsync(cancellationToken)
        //};

        return await querySpecification.ExecuteQueryAsync(query, cancellationToken: cancellationToken);
    }

    async Task<Market?> IMarketReadRepository.GetByIdAsync(long marketId, CancellationToken cancellationToken)
    {
        return await _dbContext.Markets.AsNoTracking().FirstOrDefaultAsync(t => t.Id.Equals(marketId), cancellationToken);
    }
    async Task<Market?> IMarketWriteRepository.GetByIdAsync(long marketId, CancellationToken cancellationToken)
    {
        return await _dbContext.Markets.FirstOrDefaultAsync(t => t.Id.Equals(marketId), cancellationToken);
    }

    public void Update(Market market, CancellationToken cancellationToken = default)
    {
        _dbContext.Markets.Update(market);
    }
}
