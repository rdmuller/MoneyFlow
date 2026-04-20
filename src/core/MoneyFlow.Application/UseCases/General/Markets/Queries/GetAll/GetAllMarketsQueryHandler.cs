using Mapster;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Domain.General.Entities.Markets;
using SharedKernel.Abstractions;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;

internal class GetAllMarketsQueryHandler(IMarketReadRepository marketReadRepository)
    : IQueryHandler<GetAllMarketsQuery, BaseQueryResponse<IReadOnlyList<MarketQueryDTO>>>
{
    private readonly IMarketReadRepository _marketReadRepository = marketReadRepository;

    public async Task<Result<BaseQueryResponse<IReadOnlyList<MarketQueryDTO>>>> HandleAsync(GetAllMarketsQuery request, CancellationToken cancellationToken = default)
    {
        var markets = await _marketReadRepository.GetAllAsync(request.Query, cancellationToken);

        return Result<BaseQueryResponse<IReadOnlyList<MarketQueryDTO>>>.Create(markets.Adapt<BaseQueryResponse<IReadOnlyList<MarketQueryDTO>>>());
    }
}
