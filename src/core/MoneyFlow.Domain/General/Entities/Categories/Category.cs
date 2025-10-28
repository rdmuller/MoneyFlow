using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Categories;
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
