using MoneyFlow.Domain.Entities;

namespace MoneyFlow.Domain.Security;

public interface IAccessTokenGenerator
{
    string GenerateAccessToken(User user);
}
