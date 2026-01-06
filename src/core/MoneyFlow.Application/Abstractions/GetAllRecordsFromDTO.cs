using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.Abstractions;

public abstract class GetAllRecordsFromDTO<T> : IRequest<BaseQueryResponse<IEnumerable<T>>>
{
    public QueryParams? Query { get; set; }
}