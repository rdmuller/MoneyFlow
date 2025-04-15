namespace MoneyFlow.Common;

public abstract class BaseEntityTentant : BaseEntity
{
    public long TenantId { get; set; }
}