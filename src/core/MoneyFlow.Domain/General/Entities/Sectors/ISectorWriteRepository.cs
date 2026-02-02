namespace MoneyFlow.Domain.General.Entities.Sectors;

public interface ISectorWriteRepository
{
    void Update(Sector sector, CancellationToken cancellationToken = default);

    Task CreateAsync(Sector sector, CancellationToken cancellationToken = default);

    Task<Sector?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);

    Task<Sector?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}