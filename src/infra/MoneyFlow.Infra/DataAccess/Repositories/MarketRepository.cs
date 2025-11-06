using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Domain.General.Repositories.Markets;
using MoneyFlow.Infra.DataAccess.Queries;
using SharedKernel.Communications;
using System.Linq;

namespace MoneyFlow.Infra.DataAccess.Repositories;

public class MarketRepository(ApplicationDbContext dbContext) : IMarketReadRepository, IMarketWriteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task CreateAsync(Market market, CancellationToken cancellationToken = default)
    {
        await _dbContext.Markets.AddAsync(market, cancellationToken);
    }

    public async Task<IEnumerable<Market>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Markets.AsNoTracking().AsQueryable();
        var querySpecification = new QuerySpecification<Market>(queryParams ?? new QueryParams());

        foreach (var filter in querySpecification.Filters)
            query = query.Where(filter);

        var total = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip(querySpecification.Skip)
            .Take(querySpecification.Take)
            .ToListAsync(cancellationToken);

        return data;
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
