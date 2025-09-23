namespace MoneyFlow.Domain.Common.Security;

public interface ILoggedUser
{
    Task<long> GetUserIdAsync();
}
