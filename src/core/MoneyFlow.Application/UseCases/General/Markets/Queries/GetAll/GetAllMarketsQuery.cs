using MoneyFlow.Application.DTOs.General.Markets;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;

public sealed class GetAllMarketsQuery : QueryList<MarketQueryDTO>;
