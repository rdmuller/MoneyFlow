using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Abstractions;

namespace MoneyFlow.Application.Common.Events;

internal sealed class DomainEventsDispatcher(IServiceProvider serviceProvider) : IDomainEventsDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            Type domainEventType = domainEvent.GetType();
            Type handlerType = HandlerTypeDictionary.GetOrAdd(domainEventType, eventType => typeof(IDomainEventHandler<>).MakeGenericType(eventType));
            IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is null)
                    continue;

                var handlerWrapper = HandlerWrapper.Create(handler, domainEventType);

                await handlerWrapper.HandleAsync(domainEvent, cancellationToken);
            }
        }
    }

    private abstract class HandlerWrapper
    {
        public abstract Task HandleAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);

        public static HandlerWrapper Create(object handler, Type domainEventType)
        {
            Type wrapperType = WrapperTypeDictionary.GetOrAdd(domainEventType, 
                type => typeof(HandlerWrapper<>).MakeGenericType(type));

            return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
        }
    }

    private sealed class HandlerWrapper<T>(object handler) : HandlerWrapper where T : IDomainEvent
    {
        private readonly IDomainEventHandler<T> _handler = (IDomainEventHandler<T>)handler;

        public override async Task HandleAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await _handler.HandleAsync((T)domainEvent, cancellationToken);
        }
    }
}