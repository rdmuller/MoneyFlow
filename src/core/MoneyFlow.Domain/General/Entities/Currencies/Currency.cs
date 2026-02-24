using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Currencies;

public sealed class Currency : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Symbol { get; private set; } = string.Empty;
    public bool Active { get; private set; } = true;

    public Currency()
    {
    }

    public Currency(long id, string name, string symbol, bool active, Guid? externalId = null, DateTimeOffset? createdDate = null, DateTimeOffset? updatedDate = null)
        : base(id, externalId, createdDate, updatedDate)
    {
        Name = name;
        Symbol = symbol;
        Active = active;
    }

    public static Currency Create(string name, string symbol)
    {
        Currency currency = new Currency(0, name, symbol, true, Guid.NewGuid());
        currency.CheckRequiredFields();
        return currency;
    }

    public void Update(string name, string symbol, bool? active)
    {
        Name = name;
        Symbol = symbol;

        if (active is not null)
            Active = (bool)active;

        CheckRequiredFields();
    }

    protected override void CheckRequiredFields()
    {
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Name), "Currency name must be provided");
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Symbol), "Currency symbol must be provided");
    }
}
