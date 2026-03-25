using SharedKernel.Abstractions;

namespace MoneyFlow.Domain.Abstractions.Events;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task HandleAsync(T notification, CancellationToken cancellationToken = default);
}
