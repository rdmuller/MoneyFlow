using SharedKernel.Abstractions;
using SharedKernel.BusinessRules;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace SharedKernel.Entities;

public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected BaseEntity()
    {
    }

    protected BaseEntity(long id, Guid? externalId, DateTimeOffset? createdDate, DateTimeOffset? updatedDate)
    {
        Id = id;
        ExternalId = externalId;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
    }

    public long Id { get; set; }
    public Guid? ExternalId { get; init; }// = Guid.NewGuid();
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }

    #region Soft Delete
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedOn { get; set; }
    #endregion

    #region Check integrity
    protected void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
            throw new ErrorOnValidationException([rule!.Error!]);
    }

    protected void CheckRule(bool conditionFailed, BaseError error)
    {
        if (conditionFailed)
            throw new ErrorOnValidationException([error]);
    }

    protected void CheckRequiredField(bool fieldIsEmptyOrNull, string errorMessage)
    {
        if (fieldIsEmptyOrNull)
            throw ErrorOnValidationException.RequiredFieldIsEmpty(errorMessage);
    }

    protected abstract void CheckRequiredFields();
    #endregion

    #region Domain Events
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    #endregion

}