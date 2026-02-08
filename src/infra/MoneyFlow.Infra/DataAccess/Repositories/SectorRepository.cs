using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Communications;
using System.Threading;

namespace MoneyFlow.Infra.DataAccess.Repositories;

internal class SectorRepository : BaseRepository<Sector>, ISectorWriteRepository, ISectorReadRepository
{
    public SectorRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public Task<BaseQueryResponse<IEnumerable<Sector>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    async Task<Sector?> ISectorReadRepository.GetByIdAsync(long id, CancellationToken cancellationToken)
    => await _dbContext.Sectors.AsNoTracking()
        .Include(s => s.Category)
        .FirstOrDefaultAsync(s => s.Id.Equals(id), cancellationToken);

    async Task<Sector?> ISectorReadRepository.GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
    => await _dbContext.Sectors.AsNoTracking()
        .Include(s => s.Category)
        .FirstOrDefaultAsync(s => s.ExternalId.Equals(externalId), cancellationToken);

    async Task<Sector?> ISectorWriteRepository.GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
        => await _dbContext.Sectors.FirstOrDefaultAsync(s => s.ExternalId.Equals(externalId), cancellationToken);

    async Task<Sector?> ISectorWriteRepository.GetByIdAsync(long id, CancellationToken cancellationToken)
        => await _dbContext.Sectors.FirstOrDefaultAsync(s => s.Id.Equals(id), cancellationToken);

}
