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
}