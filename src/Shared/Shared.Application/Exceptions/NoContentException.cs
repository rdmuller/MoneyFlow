using Shared.Domain;

namespace Shared.Application.Exceptions;

public class NoContentException : BaseException
{
    public NoContentException() : base(Error.None)
    {
    }
}
