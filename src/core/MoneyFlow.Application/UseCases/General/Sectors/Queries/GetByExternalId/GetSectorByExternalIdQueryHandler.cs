using Mapster;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Domain.General.Entities.Sectors;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Sectors.Queries.GetByExternalId;

internal class GetSectorByExternalIdQueryHandler(ISectorReadRepository sectorReadRepository) :
    IQueryHandler<GetSectorByExternalIdQuery, SectorQueryDTO>
{
    private readonly ISectorReadRepository _sectorReadRepository = sectorReadRepository;

    public async Task<Result<SectorQueryDTO>> HandleAsync(GetSectorByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        Sector? sector = await _sectorReadRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        return Result<SectorQueryDTO>.Create(sector.Adapt<SectorQueryDTO>());
    }
}
