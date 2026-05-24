using Mapster;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Domain.General.Entities.Currencies;
using Shared.Application.Messaging;
using Shared.Domain;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;

internal class GetAllCurrenciesQueryHandler(ICurrencyReadRepository currencyReadRepository)
    : IQueryHandler<GetAllCurrenciesQuery, BaseQueryResponse<IReadOnlyList<CurrencyQueryDTO>>>
{
    private readonly ICurrencyReadRepository _currencyReadRepository = currencyReadRepository;

    public async Task<Result<BaseQueryResponse<IReadOnlyList<CurrencyQueryDTO>>>> HandleAsync(GetAllCurrenciesQuery request, CancellationToken cancellationToken = default)
    {
        BaseQueryResponse<IEnumerable<Currency>> currencies = await _currencyReadRepository.GetAllAsync(request.Query, cancellationToken);

        return Result<BaseQueryResponse<IReadOnlyList<CurrencyQueryDTO>>>.Create(currencies.Adapt<BaseQueryResponse<IReadOnlyList<CurrencyQueryDTO>>>());
    }
}
