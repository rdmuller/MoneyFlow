using SharedKernel.Abstractions;

namespace SharedKernel.Communications;

public class BaseResponseError : BaseResponseGeneric<string>
{
    public BaseResponseError(IEnumerable<BaseError> errors)
    {
        Errors = errors;
        Data = null;
    }

    public BaseResponseError(Error error)
    {
        Errors = [BaseError.CreateError(error)];
    }
}