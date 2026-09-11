using Mapster;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Domain.General.Entities.Sectors;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Sectors.Queries.GetAll;

internal sealed class GetAllSectorsQueryHandler(ISectorReadRepository sectorReadRepository)
    : IQueryHandler<GetAllSectorsQuery, IReadOnlyList<SectorQueryDTO>>
{
    private readonly ISectorReadRepository _sectorReadRepository = sectorReadRepository;

    public async Task<Result<IReadOnlyList<SectorQueryDTO>>> HandleAsync(GetAllSectorsQuery request, CancellationToken cancellationToken = default)
    {
        Result<IEnumerable<Sector>> sectors = await _sectorReadRepository.GetAllAsync(request.Query, cancellationToken);

        if (sectors.IsFailure)
            return Result.Failure<IReadOnlyList<SectorQueryDTO>>(sectors.Errors!);

        IReadOnlyList<SectorQueryDTO> dtos = sectors.Value.Adapt<IReadOnlyList<SectorQueryDTO>>();

        return sectors.Pagination is not null
            ? Result.Success(dtos, sectors.Pagination)
            : Result.Success(dtos);
    }
}
