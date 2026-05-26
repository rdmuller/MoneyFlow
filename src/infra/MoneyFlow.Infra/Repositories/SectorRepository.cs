using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Sectors;
using MoneyFlow.Infra.DataAccess;
using MoneyFlow.Infra.DataAccess.Extensions;
using Shared.Domain;

namespace MoneyFlow.Infra.Repositories;

internal class SectorRepository : BaseRepository<Sector>, ISectorWriteRepository, ISectorReadRepository
{
    public SectorRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Result<IEnumerable<Sector>>> GetAllAsync(QueryParams? queryParams, CancellationToken cancellationToken)
    {
        IQueryable<Sector> query = _dbContext.Sectors.AsNoTracking()
            .Include(s => s.Category)
            .OrderBy(s => s.Category.Name)
            .ThenBy(s => s.Name)
            .AsQueryable();

        var querySpecification = new QuerySpecification<Sector>(queryParams ?? new QueryParams());

        return await querySpecification.ExecuteQueryAsync(query);
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
