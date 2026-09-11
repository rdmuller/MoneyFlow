using Mapster;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Domain.General.Entities.Currencies;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;

internal sealed class GetAllCurrenciesQueryHandler(ICurrencyReadRepository currencyReadRepository)
    : IQueryHandler<GetAllCurrenciesQuery, IReadOnlyList<CurrencyQueryDTO>>
{
    private readonly ICurrencyReadRepository _currencyReadRepository = currencyReadRepository;

    public async Task<Result<IReadOnlyList<CurrencyQueryDTO>>> HandleAsync(GetAllCurrenciesQuery request, CancellationToken cancellationToken = default)
    {
        Result<IEnumerable<Currency>> currencies = await _currencyReadRepository.GetAllAsync(request.Query, cancellationToken);

        if (currencies.IsFailure)
            return Result.Failure<IReadOnlyList<CurrencyQueryDTO>>(currencies.Errors!);

        IReadOnlyList<CurrencyQueryDTO> dtos = currencies.Value.Adapt<IReadOnlyList<CurrencyQueryDTO>>();

        return currencies.Pagination is not null
            ? Result.Success(dtos, currencies.Pagination)
            : Result.Success(dtos);
    }
}
