using Shared.Domain;
using Shared.Domain.Entities;

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
        var market = new Market(0, name, true, Guid.NewGuid());

        Result result = market.CheckRequiredFields();
        if (result.IsFailure)
            return Result.Failure<Market>(result.Errors!);

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
        return CheckRequiredField(string.IsNullOrWhiteSpace(Name), "Market name must be provided");
    }
}
