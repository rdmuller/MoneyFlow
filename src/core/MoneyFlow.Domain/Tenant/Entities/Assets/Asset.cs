using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Sectors;
using MoneyFlow.Domain.Tenant.Entities.Wallets;
using SharedKernel.Entities;

namespace MoneyFlow.Domain.Tenant.Entities.Assets;

public class Asset : BaseEntityTentant
{
    public string Name { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public long SectorId { get; set; }
    public long WalletId { get; set; }

    public Category? Category { get; set; }
    public Sector? Sector { get; set; }
    public Wallet? Wallet { get; set; }
}