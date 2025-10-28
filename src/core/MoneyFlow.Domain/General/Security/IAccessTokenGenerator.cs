using MoneyFlow.Domain.General.Entities.Users;

namespace MoneyFlow.Domain.General.Security;

public interface IAccessTokenGenerator
{
    TokenJwt GenerateAccessToken(User user);
}
