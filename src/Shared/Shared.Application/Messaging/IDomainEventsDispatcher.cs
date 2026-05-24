using Shared.Domain;

namespace Shared.Application.Messaging;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);

    //Task DispatchAsync<TDomainEvent>(TDomainEvent notification, CancellationToken cancellationToken = default)
    //    where TDomainEvent : IDomainEvent;
}
