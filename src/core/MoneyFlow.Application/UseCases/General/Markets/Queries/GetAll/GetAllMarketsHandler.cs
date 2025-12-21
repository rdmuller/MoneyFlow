using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Markets;
using MoneyFlow.Domain.General.Repositories.Markets;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;

internal class GetAllMarketsHandler(IMarketReadRepository marketReadRepository) : IHandler<GetAllMarketsQuery, BaseQueryResponse<IEnumerable<MarketQueryDTO>>>
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
