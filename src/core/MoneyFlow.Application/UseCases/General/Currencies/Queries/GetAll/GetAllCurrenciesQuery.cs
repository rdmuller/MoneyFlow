using MoneyFlow.Application.DTOs.General.Currencies;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;

public sealed class GetAllCurrenciesQuery : QueryList<CurrencyQueryDTO>;
