using SharedKernel.Communications;

namespace SharedKernel.BusinessRules;

public interface IBusinessRule
{
    bool IsBroken();

    BaseError? Error { get; }
}
