using MoneyFlow.Common.Entities;

namespace MoneyFlow.Domain.Entities.Investments;
public class Investment : BaseEntityTentant
{
    public string Name { get; set; } = string.Empty;
}
