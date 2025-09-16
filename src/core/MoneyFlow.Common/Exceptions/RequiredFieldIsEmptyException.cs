using MoneyFlow.Common.Communications;

namespace MoneyFlow.Common.Exceptions;
public class RequiredFieldIsEmptyException : BaseException
{
    public RequiredFieldIsEmptyException() : base()
    {
    }

    public RequiredFieldIsEmptyException(string errorCode, string errorMessage) : base(errorCode, errorMessage)
    {
    }

    public RequiredFieldIsEmptyException(IEnumerable<BaseError> errors) : base(errors)
    {
    }

    public RequiredFieldIsEmptyException(BaseError error) : base(error)
    {
    }

    public RequiredFieldIsEmptyException(string errorMessage) : base("RequiredFieldIsEmpty", errorMessage)
    {
    }
}
