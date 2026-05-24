using Shared.Domain;

namespace MoneyFlow.Domain.General.Entities.Users.Events;

public sealed record UserChangePasswordDomainEvent(User User) : IDomainEvent;