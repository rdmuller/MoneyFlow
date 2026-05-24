namespace Shared.Domain.Entities;

public abstract class BaseEntityTentant : BaseEntity
{
    public long TenantId { get; set; }
}