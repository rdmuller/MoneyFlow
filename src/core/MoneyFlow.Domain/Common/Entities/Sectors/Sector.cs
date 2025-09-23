using MoneyFlow.Common.Entities;
using MoneyFlow.Domain.Common.Entities.Categories;

namespace MoneyFlow.Domain.Common.Entities.Sectors;

public class Sector : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public long CategoryId { get; set; }

    public Category Category { get; set; } = null!;
}
