using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Sectors.Queries.GetByExternalId;

internal class GetSectorByExternalIdQueryHandler(ISectorReadRepository sectorReadRepository) : 
    IHandler<GetSectorByExternalIdQuery, BaseResponse<SectorQueryDTO>>
{
    private readonly ISectorReadRepository _sectorReadRepository = sectorReadRepository;

    public async Task<BaseResponse<SectorQueryDTO>> HandleAsync(GetSectorByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        var sector = await _sectorReadRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        if (sector is null)
            throw new NoContentException();

        return new BaseResponse<SectorQueryDTO>
        {
            Data = sector.Adapt<SectorQueryDTO>()
        };
    }
}
