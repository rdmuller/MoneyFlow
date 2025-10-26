using SharedKernel.Abstractions;

namespace MoneyFlow.Application.Common.Events;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);

    //Task DispatchAsync<TDomainEvent>(TDomainEvent notification, CancellationToken cancellationToken = default)
    //    where TDomainEvent : IDomainEvent;
}
