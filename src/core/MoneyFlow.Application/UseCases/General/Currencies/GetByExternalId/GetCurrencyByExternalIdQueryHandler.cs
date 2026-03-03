using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Currencies.GetByExternalId;

internal class GetCurrencyByExternalIdQueryHandler(
    ICurrencyReadRepository currencyReadRepository)
    : IHandler<GetCurrencyByExternalIdQuery, BaseQueryResponse<CurrencyQueryDTO>>
{
    private readonly ICurrencyReadRepository _currencyReadRepository = currencyReadRepository;

    public async Task<BaseQueryResponse<CurrencyQueryDTO>> HandleAsync(GetCurrencyByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        var currency = await _currencyReadRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);
        if (currency is null)
            throw new NoContentException();

        return new BaseQueryResponse<CurrencyQueryDTO>()
        {
            Data = currency.Adapt<CurrencyQueryDTO>()
        };
    }
}
