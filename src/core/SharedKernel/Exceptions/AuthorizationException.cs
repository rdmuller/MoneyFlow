using SharedKernel.Abstractions;
using SharedKernel.Communications;

namespace SharedKernel.Exceptions;

public class AuthorizationException : BaseException
{
    public AuthorizationException(Error error) : base(error)
    {
    }

    public AuthorizationException(IEnumerable<BaseError> errors) : base(errors)
    {
    }

    public static AuthorizationException NotAuthorized()
    {
        return new AuthorizationException("NotAuthorized", "User not authorized");
    }

    public static AuthorizationException InvalidToken()
    {
        return new AuthorizationException("InvalidToken", "Token is invalid");
    }

    public static AuthorizationException TokenExpired()
    {
        return new AuthorizationException("TokenExpired", "Token is expired");
    }

    public static AuthorizationException InvalidData(string errorMessage)
    {
        return new AuthorizationException("InvalidAuthorizationData", errorMessage);
    }
}