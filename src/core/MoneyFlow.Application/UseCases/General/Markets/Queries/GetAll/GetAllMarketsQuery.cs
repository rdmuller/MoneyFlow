using MoneyFlow.Application.DTOs.General.Markets;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;

public sealed class GetAllMarketsQuery : QueryList<MarketQueryDTO>;
