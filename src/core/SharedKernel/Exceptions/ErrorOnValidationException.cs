using SharedKernel.Communications;

namespace SharedKernel.Exceptions;

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

    public ErrorOnValidationException(BaseError error) : base(error)
    {
    }

    public static ErrorOnValidationException DataNotFound() => new ErrorOnValidationException("DataNotFound", "Tag 'Data' not found");
    public static ErrorOnValidationException RequiredFieldIsEmpty(string errorMessage) => new ErrorOnValidationException("RequiredFieldIsEmpty", errorMessage);
}