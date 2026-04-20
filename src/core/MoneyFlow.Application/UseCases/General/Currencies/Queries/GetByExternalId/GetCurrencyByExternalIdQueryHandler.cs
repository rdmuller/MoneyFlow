using Mapster;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetByExternalId;

internal class GetCurrencyByExternalIdQueryHandler(
    ICurrencyReadRepository currencyReadRepository)
    : IQueryHandler<GetCurrencyByExternalIdQuery, CurrencyQueryDTO>
{
    private readonly ICurrencyReadRepository _currencyReadRepository = currencyReadRepository;

    public async Task<Result<CurrencyQueryDTO>> HandleAsync(GetCurrencyByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        var currency = await _currencyReadRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        return Result<CurrencyQueryDTO>.Create(currency.Adapt<CurrencyQueryDTO>());
    }
}
