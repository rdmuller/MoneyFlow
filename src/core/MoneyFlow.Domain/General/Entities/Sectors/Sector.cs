using MoneyFlow.Domain.General.Entities.Categories;
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

    private Sector(long id, string name, long categoryId, bool active = true, 
        Guid? externalId = null, DateTimeOffset? createdDate = null, DateTimeOffset? updatedDate = null)
        : base(id, externalId, createdDate, updatedDate)
    {
        Name = name;
        CategoryId = categoryId;
        Active = active;
    }

    public static Sector Create(string name, Category category)
    {
        Sector sector = new Sector(0, name, category.Id, true, Guid.CreateVersion7());

        sector.CheckRequiredFields();
        sector.CheckForeignKeys(category);

        return sector;
    }

    public void Update(string name, Category category, bool active = true)
    {
        Name = name;
        Active = active;
        CategoryId = category is not null ? category.Id : 0;

        this.CheckRequiredFields();
        this.CheckForeignKeys(category!);
    }

    private void CheckForeignKeys(Category category)
    {
        CheckRule(new CategoryMustBeActiveBusinessRule(category));
    }

    protected override void CheckRequiredFields()
    {
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Name), "Sector name is required");
        CheckRequiredField((this.CategoryId.Equals(0)), "Category is required");
    }
}