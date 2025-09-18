using MoneyFlow.Common.Entities;

namespace MoneyFlow.Domain.Entities.InvestmentTypes;
public class ActiveType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
