using SharedKernel.BusinessRules;
using SharedKernel.Communications;
using SharedKernel.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Entities;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public Guid ExternalId { get; set; } = Guid.NewGuid();
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }

    protected void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
            throw new ErrorOnValidationException([rule!.Error!]);
    }

    protected void CheckRule(bool condition, BaseError error)
    {
        if (condition)
            throw new ErrorOnValidationException([error]);
    }
}