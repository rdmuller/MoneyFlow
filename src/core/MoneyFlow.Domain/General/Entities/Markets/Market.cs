using SharedKernel.Abstractions;
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

    public static Result<Market> Create(string name)
    {
        Market market = new Market(0, name, true, Guid.NewGuid());

        var result = market.CheckRequiredFields();
        if (result.IsFailure)
            return Result.Failure<Market>(result.Error);

        return Result.Success(market);
    }

    public Result Update(string name, bool? active)
    {
        Name = name;

        if (active is not null)
            Active = (bool)active;

        return CheckRequiredFields();
    }

    protected override Result CheckRequiredFields()
    {
        return CheckRequiredField(string.IsNullOrWhiteSpace(this.Name), "Market name must be provided");
    }
}
