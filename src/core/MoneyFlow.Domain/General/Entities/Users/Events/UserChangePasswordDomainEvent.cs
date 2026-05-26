using Shared.Domain;

namespace MoneyFlow.Domain.General.Entities.Users.Events;

public sealed class UserChangePasswordDomainEvent(User user) : DomainEvent
{
    public User User { get; set; } = user;
}
