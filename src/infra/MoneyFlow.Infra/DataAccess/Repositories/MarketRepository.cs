using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.Common.Entities.Markets;
using MoneyFlow.Domain.Common.Repositories.Markets;

namespace MoneyFlow.Infra.DataAccess.Repositories;

public class MarketRepository(ApplicationDbContext dbContext) : IMarketReadRepository, IMarketWriteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task CreateMarketAsync(Market market, CancellationToken cancellationToken = default)
    {
        await _dbContext.Markets.AddAsync(market, cancellationToken);
    }

    public Task<IEnumerable<Market>> GetAllMarketsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    async Task<Market?> IMarketReadRepository.GetMarketByIdAsync(long marketId, CancellationToken cancellationToken)
    {
        return await _dbContext.Markets.AsNoTracking().FirstAsync(t => t.Id.Equals(marketId), cancellationToken);
    }
    async Task<Market> IMarketWriteRepository.GetMarketByIdAsync(long marketId, CancellationToken cancellationToken)
    {
        return await _dbContext.Markets.FirstAsync(t => t.Id.Equals(marketId), cancellationToken);
    }

    public void UpdateMarket(Market market, CancellationToken cancellationToken = default)
    {
        _dbContext.Markets.Update(market);
    }
}
