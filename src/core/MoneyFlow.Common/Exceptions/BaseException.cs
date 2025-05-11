using MoneyFlow.Common.Communications;

namespace MoneyFlow.Common.Exceptions;

public abstract class BaseException //: Exception
{
    public IEnumerable<BaseError> Errors { get; protected set; } = [];

    public BaseException()
    {
    }

    public BaseException(IEnumerable<BaseError> errors)
    {
        Errors = errors;
    }

    public BaseException(string errorCode, string errorMessage)
    {
        Errors = new List<BaseError>
        {
            new BaseError
            {
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            }
        };
    }

}