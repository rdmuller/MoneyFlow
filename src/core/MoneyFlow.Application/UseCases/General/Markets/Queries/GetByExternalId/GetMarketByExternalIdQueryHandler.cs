using Mapster;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Domain.General.Entities.Markets;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetByExternalId;

internal class GetMarketByExternalIdQueryHandler(IMarketReadRepository marketRepository)
    : IQueryHandler<GetMarketByExternalIdQuery, MarketQueryDTO>
{
    private readonly IMarketReadRepository _marketRepository = marketRepository;

    public async Task<Result<MarketQueryDTO>> HandleAsync(GetMarketByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        Market? market = await _marketRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        return Result<MarketQueryDTO>.Create(market.Adapt<MarketQueryDTO>());
    }
}
