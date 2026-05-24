namespace Shared.Domain.BusinessRules;

public interface IBusinessRule
{
    bool IsBroken();

    Error? Error { get; }
}
