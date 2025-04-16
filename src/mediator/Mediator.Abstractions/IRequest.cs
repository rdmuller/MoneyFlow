namespace Mediator.Abstractions;

public interface IRequest<TResponse>
    where TResponse : IResponse;