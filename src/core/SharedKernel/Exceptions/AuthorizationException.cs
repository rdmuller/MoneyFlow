using SharedKernel.Abstractions;
using SharedKernel.Communications;

namespace SharedKernel.Exceptions;

public class AuthorizationException : BaseException
{
    public AuthorizationException(Error error) : base(error)
    {
    }

    public AuthorizationException(IEnumerable<Error> errors) : base(errors)
    {
    }

    public static AuthorizationException NotAuthorized()
        => new AuthorizationException(Error.NotAuthorized("User not authorized"));

    public static AuthorizationException InvalidToken() 
        => new AuthorizationException(Error.NotAuthorized("Token is invalid"));

    public static AuthorizationException TokenExpired() 
        => new AuthorizationException(Error.NotAuthorized("Token is expired"));

    public static AuthorizationException InvalidData(string errorMessage) 
        => new AuthorizationException(Error.NotAuthorized(errorMessage));
}