using Shared.Domain;

namespace Shared.Application.Messaging;

public interface ICommand<TRequest> : IRequest<Result<TRequest>>
{
}

public interface ICommand : IRequest<Result>
{
}
