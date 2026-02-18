using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Sectors.Queries.GetAll;

internal class GetAllSectorsQueryHandler(ISectorReadRepository sectorReadRepository)
    : IHandler<GetAllSectorsQuery, BaseQueryResponse<IEnumerable<SectorQueryDTO>>>
{
    private readonly ISectorReadRepository _sectorReadRepository = sectorReadRepository;

    public async Task<BaseQueryResponse<IEnumerable<SectorQueryDTO>>> HandleAsync(GetAllSectorsQuery request, CancellationToken cancellationToken = default)
    {
        var sectors = await _sectorReadRepository.GetAllAsync(request.Query, cancellationToken);

        if (sectors.TotalRows == 0)
            throw new NoContentException();

        return sectors.Adapt<BaseQueryResponse<IEnumerable<SectorQueryDTO>>>();
    }
}
