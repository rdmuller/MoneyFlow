using Mapster;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Domain.General.Entities.Currencies;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetByExternalId;

internal class GetCurrencyByExternalIdQueryHandler(
    ICurrencyReadRepository currencyReadRepository)
    : IQueryHandler<GetCurrencyByExternalIdQuery, CurrencyQueryDTO>
{
    private readonly ICurrencyReadRepository _currencyReadRepository = currencyReadRepository;

    public async Task<Result<CurrencyQueryDTO>> HandleAsync(GetCurrencyByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        Currency? currency = await _currencyReadRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        return Result<CurrencyQueryDTO>.Create(currency.Adapt<CurrencyQueryDTO>());
    }
}
