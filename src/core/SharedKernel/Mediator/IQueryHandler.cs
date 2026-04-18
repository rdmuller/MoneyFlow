using SharedKernel.Abstractions;

namespace SharedKernel.Mediator;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
