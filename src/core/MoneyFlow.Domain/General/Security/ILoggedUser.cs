namespace MoneyFlow.Domain.General.Security;

public interface ILoggedUser
{
    Task<long> GetUserIdAsync();
}
