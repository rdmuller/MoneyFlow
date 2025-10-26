using SharedKernel.Abstractions;

namespace MoneyFlow.Domain.Common.Entities.Users;

public sealed class UserChangePasswordDomainEvent(User user) : IDomainEvent
{
    public User User = user;
}
