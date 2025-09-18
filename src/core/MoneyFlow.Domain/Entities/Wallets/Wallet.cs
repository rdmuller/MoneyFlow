using MoneyFlow.Common.Entities;

namespace MoneyFlow.Domain.Entities.Wallets;
public class Wallet : BaseEntityTentant
{
    public string name { get; set; } = string.Empty;
}
