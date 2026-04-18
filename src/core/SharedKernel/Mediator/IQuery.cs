using SharedKernel.Abstractions;

namespace SharedKernel.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
