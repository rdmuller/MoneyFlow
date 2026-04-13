using SharedKernel.Abstractions;

namespace SharedKernel.Mediator;

public interface ICommand<TRequest> : IRequest<Result<TRequest>>
{
}

public interface ICommand : IRequest<Result>
{
}
