using MoneyFlow.Application.DTOs.General.Currencies;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetByExternalId;

public sealed record GetCurrencyByExternalIdQuery(Guid ExternalId) : IQuery<CurrencyQueryDTO>;