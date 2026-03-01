using MoneyFlow.Application.Abstractions;
using MoneyFlow.Application.DTOs.General.Currencies;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;

public sealed class GetAllCurrenciesQuery : GetAllRecordsFromDTO<CurrencyQueryDTO>
{
}
