using MoneyFlow.Common.Communications;

namespace MoneyFlow.Common.Exceptions;

public class ErrorOnValidationException : BaseException
{
    public ErrorOnValidationException()
    {
        Errors = new List<BaseError>();
    }

    public ErrorOnValidationException(IEnumerable<BaseError> errors)
    {
        Errors = errors;
    }

    public ErrorOnValidationException(string errorCode, string errorMessage)
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

    public static ErrorOnValidationException DataNotFound()
    {
        return new ErrorOnValidationException("DataNotFound", "Tag 'Data' not found");
    }
}