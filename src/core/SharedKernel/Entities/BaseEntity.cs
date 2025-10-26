namespace SharedKernel.Entities;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public Guid ExternalId { get; set; } = Guid.NewGuid();
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}