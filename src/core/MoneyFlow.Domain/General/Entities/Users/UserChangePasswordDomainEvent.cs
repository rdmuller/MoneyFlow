using SharedKernel.Abstractions;

namespace MoneyFlow.Domain.General.Entities.Users;

public sealed class UserChangePasswordDomainEvent(User user) : IDomainEvent
{
    public User User = user;
}
