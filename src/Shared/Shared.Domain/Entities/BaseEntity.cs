using Shared.Domain.BusinessRules;

namespace Shared.Domain.Entities;

public abstract class BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = new();

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
    protected Result CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
            return Result.Failure(rule!.Error!);

        return Result.Success();
    }

    protected Result CheckRule(bool conditionFailed, Error error)
    {
        if (conditionFailed)
            return Result.Failure(error);

        return Result.Success();
    }

    protected Result CheckRequiredField(bool fieldIsEmptyOrNull, string errorMessage)
    {
        if (fieldIsEmptyOrNull)
            return Result.Failure(Error.RequiredFieldIsEmpty(errorMessage));

        return Result.Success();
    }

    protected abstract Result CheckRequiredFields();
    #endregion

    #region Domain Events
    public IReadOnlyList<DomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    #endregion

}
