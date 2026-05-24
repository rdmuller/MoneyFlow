using MoneyFlow.Application.DTOs.General.Currencies;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;

public sealed class GetAllCurrenciesQuery : QueryList<CurrencyQueryDTO>;
