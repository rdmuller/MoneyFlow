using Mediator.Abstractions;

namespace Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        var handler = _serviceProvider.GetService(handlerType);

        //foreach (var handler in handlers)
        //{
            await (Task)handlerType
                .GetMethod("Handle")!
                .Invoke(handler, new object[] { notification, cancellationToken })!;
        //}
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);

        if (handler is null)
            throw new InvalidOperationException($"Handler not found for {requestType.Name}");

        var method = handlerType.GetMethod("HandleAsync");
        if (method is null)
            throw new InvalidOperationException($"Method HandleAsync not found for {requestType.Name}");

        var result = method.Invoke(handler, [request, cancellationToken]);
        if (result is not Task<TResponse> task)
            throw new InvalidOperationException($"Method returned unexpected type {result} for {requestType.Name}");

        return await task;

        /*return await (Task<TResponse>)handlerType
            .GetMethod("Handle")!
            .Invoke(handler, new object[] { request, cancellationToken })!;*/
    }
}