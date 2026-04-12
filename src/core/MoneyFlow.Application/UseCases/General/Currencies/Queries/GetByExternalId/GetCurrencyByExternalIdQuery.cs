using MoneyFlow.Application.DTOs.General.Currencies;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Queries.GetByExternalId;

public sealed record class GetCurrencyByExternalIdQuery(Guid ExternalId) : IRequest<BaseQueryResponse<CurrencyQueryDTO>>;