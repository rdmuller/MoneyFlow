using SharedKernel.Abstractions;

namespace MoneyFlow.Application.Common.Events;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task HandleAsync(T notification, CancellationToken cancellationToken = default);
}
