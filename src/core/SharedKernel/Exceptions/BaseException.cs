using SharedKernel.Abstractions;
using SharedKernel.Communications;

namespace SharedKernel.Exceptions;

public abstract class BaseException : Exception
{
    public IEnumerable<BaseError> Errors { get; protected set; } = [];

    public BaseException(IEnumerable<Error> errors)
    {
        Errors = errors.Select(BaseError.CreateError);
    }

    public BaseException(Error error)
    {
        Errors = [BaseError.CreateError(error)];
    }
}