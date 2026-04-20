using Mapster;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Domain.General.Entities.Markets;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetByExternalId;

internal class GetMarketByExternalIdQueryHandler(IMarketReadRepository marketRepository)
    : IQueryHandler<GetMarketByExternalIdQuery, MarketQueryDTO>
{
    private readonly IMarketReadRepository _marketRepository = marketRepository;

    public async Task<Result<MarketQueryDTO>> HandleAsync(GetMarketByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        var market = await _marketRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        return Result<MarketQueryDTO>.Create(market.Adapt<MarketQueryDTO>());
    }
}
