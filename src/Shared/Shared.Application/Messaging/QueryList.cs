using Shared.Domain;

namespace Shared.Application.Messaging;

public abstract class QueryList<T> : IQuery<IReadOnlyList<T>>
{
    public QueryParams? Query { get; set; }
}
