using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Sectors;

public class Sector : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public long CategoryId { get; set; }

    public Category Category { get; set; } = null!;
}
