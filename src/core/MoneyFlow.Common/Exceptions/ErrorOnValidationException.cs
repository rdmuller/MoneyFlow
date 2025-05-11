using MoneyFlow.Common.Communications;

namespace MoneyFlow.Common.Exceptions;

public class ErrorOnValidationException : BaseException
{
    public ErrorOnValidationException() : base()
    {
    }

    public ErrorOnValidationException(string errorCode, string errorMessage) : base(errorCode, errorMessage)
    {
    }

    public ErrorOnValidationException(IEnumerable<BaseError> errors) : base(errors)
    {
    }

    public static ErrorOnValidationException DataNotFound()
    {
        return new ErrorOnValidationException("DataNotFound", "Tag 'Data' not found");
    }
}