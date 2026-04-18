using SharedKernel.Communications;

namespace SharedKernel.Mediator;

public abstract class QueryList<T> : IQuery<BaseQueryResponse<IReadOnlyList<T>>>
{
    public QueryParams? Query { get; set; }
}