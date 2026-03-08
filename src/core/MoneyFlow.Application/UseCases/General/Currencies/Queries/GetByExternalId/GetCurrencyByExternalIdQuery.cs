using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Currencies;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetByExternalId;

public sealed record class GetCurrencyByExternalIdQuery (Guid ExternalId) : IRequest<BaseQueryResponse<CurrencyQueryDTO>>;