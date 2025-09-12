using MoneyFlow.Common.Abstractions;

namespace MoneyFlow.Domain.Entities.Users;

public sealed class UserChangePasswordDomainEvent(User user) : IDomainEvent
{
    public User User = user;
}
