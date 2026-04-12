using MoneyFlow.Application.DTOs.General.Markets;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetByExternalId;

public sealed record GetMarketByExternalIdQuery(Guid ExternalId) : IRequest<BaseResponse<MarketQueryDTO>>;