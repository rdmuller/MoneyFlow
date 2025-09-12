using MoneyFlow.Domain.Entities.Users;

namespace MoneyFlow.Domain.Security;

public interface IAccessTokenGenerator
{
    TokenJwt GenerateAccessToken(User user);
}
