using MoneyFlow.Domain.Entities;

namespace MoneyFlow.Domain.Security;

public interface IAccessTokenGenerator
{
    TokenJwt GenerateAccessToken(User user);
}
