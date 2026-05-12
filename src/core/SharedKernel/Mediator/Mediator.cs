using System.Reflection;

namespace SharedKernel.Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        Type requestType = request.GetType();
        Type handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        object? handler = _serviceProvider.GetService(handlerType);

        if (handler is null)
            throw new InvalidOperationException($"Handler not found for {requestType.Name}");

        MethodInfo? method = handlerType.GetMethod("HandleAsync");
        if (method is null)
            throw new InvalidOperationException($"Method HandleAsync not found for {requestType.Name}");

        object? result = method.Invoke(handler, [request, cancellationToken]);
        if (result is not Task<TResponse> task)
            throw new InvalidOperationException($"Method returned unexpected type {result} for {requestType.Name}");

        return await task;

        /*return await (Task<TResponse>)handlerType
            .GetMethod("Handle")!
            .Invoke(handler, new object[] { request, cancellationToken })!;*/
    }
}