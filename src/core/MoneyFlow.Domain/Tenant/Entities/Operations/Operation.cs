using MoneyFlow.Domain.Tenant.Entities.Assets;
using SharedKernel.Entities;

namespace MoneyFlow.Domain.Tenant.Entities.Operations;

public class Operation : BaseEntityTentant
{
    public string Name { get; set; } = string.Empty;
    public int AssetId { get; set; }

    public Asset? Asset { get; set; }
}
