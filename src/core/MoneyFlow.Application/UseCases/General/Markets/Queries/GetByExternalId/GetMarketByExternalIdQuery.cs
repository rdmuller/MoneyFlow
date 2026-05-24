using MoneyFlow.Application.DTOs.General.Markets;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetByExternalId;

public sealed record GetMarketByExternalIdQuery(Guid ExternalId) : IQuery<MarketQueryDTO>;