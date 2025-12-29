using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Markets;

public sealed class Market : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public bool Active { get; private set; } = true;

    private Market()
    {

    }

    public Market(long id, string name, bool active, Guid? externalId = null, DateTimeOffset? createdDate = null, DateTimeOffset? updatedDate = null)
        : base(id, externalId, createdDate, updatedDate)
    {
        Name = name;
        Active = active;
    }

    public static Market Create(string name)
    {
        Market market = new Market(0, name, true, Guid.NewGuid());

        market.CheckRequiredFields();

        return market;
    }

    protected override void CheckRequiredFields()
    {
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Name), "Market name must be provided");
    }
}
