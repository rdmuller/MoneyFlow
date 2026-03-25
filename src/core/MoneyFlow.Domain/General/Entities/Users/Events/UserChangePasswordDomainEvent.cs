using SharedKernel.Abstractions;

namespace MoneyFlow.Domain.General.Entities.Users.Events;

public sealed record UserChangePasswordDomainEvent(User User) : IDomainEvent;