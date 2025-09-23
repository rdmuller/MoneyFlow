using MoneyFlow.Domain.Common.Entities.Users;

namespace MoneyFlow.Domain.Common.Security;

public interface IAccessTokenGenerator
{
    TokenJwt GenerateAccessToken(User user);
}
