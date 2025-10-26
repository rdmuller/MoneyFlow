using SharedKernel.Communications;

namespace SharedKernel.Exceptions;

public class AuthorizationException : BaseException
{
    public AuthorizationException() : base()
    {
    }

    public AuthorizationException(string errorCode, string errorMessage) : base(errorCode, errorMessage)
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