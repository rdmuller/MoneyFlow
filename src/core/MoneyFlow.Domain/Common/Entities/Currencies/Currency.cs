using MoneyFlow.Common.Entities;

namespace MoneyFlow.Domain.Common.Entities.Currencies;

public class Currency : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
