using SharedKernel.Abstractions;
using SharedKernel.Communications;

namespace SharedKernel.BusinessRules;

public interface IBusinessRule
{
    bool IsBroken();

    Error? Error { get; }
}
