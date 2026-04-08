using SharedKernel.Abstractions;

namespace SharedKernel.Exceptions;

public class ErrorOnValidationException : BaseException
{
    public ErrorOnValidationException(Error error) : base(error)
    {
    }

    public ErrorOnValidationException(IEnumerable<Error> errors) : base(errors)
    {
    }

    public static ErrorOnValidationException DataNotFound() 
        => new ErrorOnValidationException(Error.DataTagNotFound);

    public static ErrorOnValidationException RequiredFieldIsEmpty(string errorMessage) 
        => new ErrorOnValidationException(Error.RequiredFieldisEmpty(errorMessage));

    public static ErrorOnValidationException InactiveForeignKey(string errorMessage) 
        => new ErrorOnValidationException(Error.InactiveForeignKey(errorMessage));
}