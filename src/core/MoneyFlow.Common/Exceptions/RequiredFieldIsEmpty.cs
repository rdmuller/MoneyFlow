using MoneyFlow.Common.Communications;

namespace MoneyFlow.Common.Exceptions;
public class RequiredFieldIsEmpty : BaseException
{
    public RequiredFieldIsEmpty() : base()
    {
    }

    public RequiredFieldIsEmpty(string errorCode, string errorMessage) : base(errorCode, errorMessage)
    {
    }

    public RequiredFieldIsEmpty(IEnumerable<BaseError> errors) : base(errors)
    {
    }

    public RequiredFieldIsEmpty(BaseError error) : base(error)
    {
    }

    public RequiredFieldIsEmpty(string errorMessage) : base("RequiredFieldIsEmpty", errorMessage)
    {
    }
}
