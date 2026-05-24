using SharedKernel.Communications;

namespace Shared.Application.Messaging;

public abstract class QueryList<T> : IQuery<BaseQueryResponse<IReadOnlyList<T>>>
{
    public QueryParams? Query { get; set; }
}