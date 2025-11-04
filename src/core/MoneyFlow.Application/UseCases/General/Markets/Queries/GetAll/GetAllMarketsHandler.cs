using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Markets;
using MoneyFlow.Domain.General.Repositories.Markets;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;
internal class GetAllMarketsHandler(IMarketReadRepository marketReadRepository) : IHandler<GetAllMarketsQuery, BaseQueryResponse<List<MarketQueryDTO>>>
{
    private readonly IMarketReadRepository _marketReadRepository = marketReadRepository;

    public async Task<BaseQueryResponse<List<MarketQueryDTO>>> HandleAsync(GetAllMarketsQuery request, CancellationToken cancellationToken = default)
    {
        var markets = await _marketReadRepository.GetAllAsync(cancellationToken);

        if (markets.Count() == 0)
            throw new NoContentException();

        return new BaseQueryResponse<List<MarketQueryDTO>>
        {
            Data = markets.Adapt<List<MarketQueryDTO>>()
        };
    }
}
