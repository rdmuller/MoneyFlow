using Shared.Domain;

namespace Shared.Application.Messaging;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task HandleAsync(T notification, CancellationToken cancellationToken = default);
}
