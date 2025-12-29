using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Categories;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public bool Active { get; private set; } = true;

    private Category()
    {
        // Only for EF
    }

    public Category(long id, string name, bool active, Guid? externalId = null, DateTimeOffset? createdDate = null, DateTimeOffset? updatedDate = null)
        : base(id, externalId, createdDate, updatedDate)
    {
        Name = name;
        Active = active;
    }

    public static Category Create(string name)
    {
        Category category = new Category(0, name, true, Guid.NewGuid());

        category.CheckRequiredFields();

        return category;
    }

    protected override void CheckRequiredFields()
    {
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Name), "Category name must be provided");
    } 
}