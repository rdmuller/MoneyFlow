using SharedKernel.Abstractions;

namespace SharedKernel.Communications;

public class BaseResponseError : BaseResponseGeneric<string>
{
    public BaseResponseError(IEnumerable<BaseError> errors)
    {
        Errors = errors;
        Data = null;
    }

    public BaseResponseError(string errorCode, string errorMessage)
    {
        Errors = [new BaseError(errorCode, errorMessage)];
    }

    public BaseResponseError(string errorCode, string errorMessage, string errorDescription)
    {
        Errors = [new BaseError(errorCode, errorMessage, errorDescription)];
    }
}