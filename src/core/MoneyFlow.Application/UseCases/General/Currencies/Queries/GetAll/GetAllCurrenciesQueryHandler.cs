using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;

internal class GetAllCurrenciesQueryHandler(ICurrencyReadRepository currencyReadRepository)
    : IHandler<GetAllCurrenciesQuery, BaseQueryResponse<IEnumerable<CurrencyQueryDTO>>>
{
    private readonly ICurrencyReadRepository _currencyReadRepository = currencyReadRepository;

    public async Task<BaseQueryResponse<IEnumerable<CurrencyQueryDTO>>> HandleAsync(GetAllCurrenciesQuery request, CancellationToken cancellationToken = default)
    {
        var currencies = await _currencyReadRepository.GetAllAsync(request.Query, cancellationToken);

        if (currencies.TotalRows == 0)
            throw new NoContentException();

        return currencies.Adapt<BaseQueryResponse<IEnumerable<CurrencyQueryDTO>>>();
    }
}
