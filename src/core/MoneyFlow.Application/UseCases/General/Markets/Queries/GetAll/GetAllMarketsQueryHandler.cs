using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Domain.General.Entities.Markets;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;

internal class GetAllMarketsQueryHandler(IMarketReadRepository marketReadRepository) : IHandler<GetAllMarketsQuery, BaseQueryResponse<IEnumerable<MarketQueryDTO>>>
{
    private readonly IMarketReadRepository _marketReadRepository = marketReadRepository;

    public async Task<BaseQueryResponse<IEnumerable<MarketQueryDTO>>> HandleAsync(GetAllMarketsQuery request, CancellationToken cancellationToken = default)
    {
        var markets = await _marketReadRepository.GetAllAsync(request.Query, cancellationToken);

        if (markets.TotalRows == 0)
            throw new NoContentException();

        return markets.Adapt<BaseQueryResponse<IEnumerable<MarketQueryDTO>>>();
    }
}
