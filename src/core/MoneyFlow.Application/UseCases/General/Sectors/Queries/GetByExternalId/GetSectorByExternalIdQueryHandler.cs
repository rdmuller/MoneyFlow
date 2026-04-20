using Mapster;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Queries.GetByExternalId;

internal class GetSectorByExternalIdQueryHandler(ISectorReadRepository sectorReadRepository) :
    IQueryHandler<GetSectorByExternalIdQuery, SectorQueryDTO>
{
    private readonly ISectorReadRepository _sectorReadRepository = sectorReadRepository;

    public async Task<Result<SectorQueryDTO>> HandleAsync(GetSectorByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        var sector = await _sectorReadRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        return Result<SectorQueryDTO>.Create(sector.Adapt<SectorQueryDTO>());
    }
}
