using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Currencies;

public sealed class Currency : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;

    public Currency()
    {
    }

    public Currency(long id, string name, bool active, Guid? externalId = null, DateTimeOffset? createdDate = null, DateTimeOffset? updatedDate = null)
        : base(id, externalId, createdDate, updatedDate)
    {
        Name = name;
        Active = active;
    }

    public static Currency Create(string name)
    {
        Currency currency = new Currency(0, name, true, Guid.NewGuid());
        currency.CheckRequiredFields();
        return currency;
    }

    protected override void CheckRequiredFields()
    {
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Name), "Currency name must be provided");
    }
}
