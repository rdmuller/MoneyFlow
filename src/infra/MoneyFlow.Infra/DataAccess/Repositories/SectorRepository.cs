using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Sectors;

namespace MoneyFlow.Infra.DataAccess.Repositories;

internal class SectorRepository : BaseRepository<Sector>, ISectorWriteRepository
{
    public SectorRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Sector?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default)
        => await _dbContext.Sectors.FirstOrDefaultAsync(s => s.ExternalId.Equals(externalId), cancellationToken);

    public async Task<Sector?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await _dbContext.Sectors.FirstOrDefaultAsync(s => s.Id.Equals(id), cancellationToken);
}
