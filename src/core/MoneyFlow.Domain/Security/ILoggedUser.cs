using MoneyFlow.Domain.Entities;

namespace MoneyFlow.Domain.Security;

public interface ILoggedUser
{
    Task<User> GetUserAsync();
}
