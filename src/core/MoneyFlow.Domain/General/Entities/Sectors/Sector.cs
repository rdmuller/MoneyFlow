using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Communications;
using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Sectors;

public sealed class Sector : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public bool Active { get; private set; } = true;
    public long CategoryId { get; private set; }

    public Category Category { get; set; } = null!;

    private Sector()
    {
        // Only for EF
    }

    private Sector(string name, long categoryId, bool active = true)
    {
        this.CheckRule(string.IsNullOrWhiteSpace(name), BaseError.ValidationError("Sector name is required"));

        Name = name;
        CategoryId = categoryId;
        Active = active;
    }

    public static Sector Create(string name, long categoryId, bool active = true)
    {
        return new Sector(name, categoryId, active);
    }

    protected override void CheckRequiredFields()
    {
        throw new NotImplementedException();
    }
}
