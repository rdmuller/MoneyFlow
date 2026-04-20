using Mapster;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Abstractions;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Queries.GetAll;

internal class GetAllSectorsQueryHandler(ISectorReadRepository sectorReadRepository)
    : IQueryHandler<GetAllSectorsQuery, BaseQueryResponse<IReadOnlyList<SectorQueryDTO>>>
{
    private readonly ISectorReadRepository _sectorReadRepository = sectorReadRepository;

    public async Task<Result<BaseQueryResponse<IReadOnlyList<SectorQueryDTO>>>> HandleAsync(GetAllSectorsQuery request, CancellationToken cancellationToken = default)
    {
        var sectors = await _sectorReadRepository.GetAllAsync(request.Query, cancellationToken);

        return Result<BaseQueryResponse<IReadOnlyList<SectorQueryDTO>>>.Create(sectors.Adapt<BaseQueryResponse<IReadOnlyList<SectorQueryDTO>>>());
    }
}
