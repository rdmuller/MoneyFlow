using SharedKernel.Abstractions;

namespace SharedKernel.Exceptions;

public class NoContentException : BaseException
{
    public NoContentException() : base(Error.None)
    {
    }
}
