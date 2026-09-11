using Mapster;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Domain.General.Entities.Markets;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;

internal sealed class GetAllMarketsQueryHandler(IMarketReadRepository marketReadRepository)
    : IQueryHandler<GetAllMarketsQuery, IReadOnlyList<MarketQueryDTO>>
{
    private readonly IMarketReadRepository _marketReadRepository = marketReadRepository;

    public async Task<Result<IReadOnlyList<MarketQueryDTO>>> HandleAsync(GetAllMarketsQuery request, CancellationToken cancellationToken = default)
    {
        Result<IEnumerable<Market>> markets = await _marketReadRepository.GetAllAsync(request.Query, cancellationToken);

        if (markets.IsFailure)
            return Result.Failure<IReadOnlyList<MarketQueryDTO>>(markets.Errors!);

        IReadOnlyList<MarketQueryDTO> dtos = markets.Value.Adapt<IReadOnlyList<MarketQueryDTO>>();

        return markets.Pagination is not null
            ? Result.Success(dtos, markets.Pagination)
            : Result.Success(dtos);
    }
}
