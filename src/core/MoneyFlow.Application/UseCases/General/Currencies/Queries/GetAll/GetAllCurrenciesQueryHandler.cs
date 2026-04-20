using Mapster;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Abstractions;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;

internal class GetAllCurrenciesQueryHandler(ICurrencyReadRepository currencyReadRepository)
    : IQueryHandler<GetAllCurrenciesQuery, BaseQueryResponse<IReadOnlyList<CurrencyQueryDTO>>>
{
    private readonly ICurrencyReadRepository _currencyReadRepository = currencyReadRepository;

    public async Task<Result<BaseQueryResponse<IReadOnlyList<CurrencyQueryDTO>>>> HandleAsync(GetAllCurrenciesQuery request, CancellationToken cancellationToken = default)
    {
        var currencies = await _currencyReadRepository.GetAllAsync(request.Query, cancellationToken);

        return Result<BaseQueryResponse<IReadOnlyList<CurrencyQueryDTO>>>.Create(currencies.Adapt<BaseQueryResponse<IReadOnlyList<CurrencyQueryDTO>>>());
    }
}
