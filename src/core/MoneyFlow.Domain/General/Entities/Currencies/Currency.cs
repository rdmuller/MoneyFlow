using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Currencies;

public class Currency : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
