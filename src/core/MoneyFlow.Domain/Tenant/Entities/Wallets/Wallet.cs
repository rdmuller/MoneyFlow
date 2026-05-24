using Shared.Domain;
using Shared.Domain.Entities;

namespace MoneyFlow.Domain.Tenant.Entities.Wallets;

public class Wallet : BaseEntityTentant
{
    public string Name { get; set; } = string.Empty;

    protected override Result CheckRequiredFields()
    {
        return Result.Failure(new Error("NotImplemented", "MoneyFlow.Domain.Tenant.Entities.Wallets"));
    }
}
