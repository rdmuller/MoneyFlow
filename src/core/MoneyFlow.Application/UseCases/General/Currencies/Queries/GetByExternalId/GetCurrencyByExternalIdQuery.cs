using MoneyFlow.Application.DTOs.General.Currencies;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetByExternalId;

public sealed record GetCurrencyByExternalIdQuery(Guid ExternalId) : IQuery<CurrencyQueryDTO>;