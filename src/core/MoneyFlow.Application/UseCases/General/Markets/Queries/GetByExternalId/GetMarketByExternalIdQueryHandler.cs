using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Domain.General.Entities.Markets;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetByExternalId;

internal class GetMarketByExternalIdQueryHandler(IMarketReadRepository marketRepository) : IHandler<GetMarketByExternalIdQuery, BaseResponse<MarketQueryDTO>>
{
    private readonly IMarketReadRepository _marketRepository = marketRepository;

    public async Task<BaseResponse<MarketQueryDTO>> HandleAsync(GetMarketByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        var market = await _marketRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        if (market is null)
            throw new NoContentException();

        return new BaseResponse<MarketQueryDTO>
        {
            Data = market.Adapt<MarketQueryDTO>()
        };
    }
}
