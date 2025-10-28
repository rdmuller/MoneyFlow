using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Markets;

public class Market : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
