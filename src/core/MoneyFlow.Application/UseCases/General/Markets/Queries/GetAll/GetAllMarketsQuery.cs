using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Markets;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;
public class GetAllMarketsQuery : IRequest<BaseQueryResponse<List<MarketQueryDTO>>>
{
    public QueryParams? Query { get; set; }
}
