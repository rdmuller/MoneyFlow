using Shared.Domain;
using Shared.Domain.Entities;

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

    public static Result<Category> Create(string name)
    {
        var category = new Category(0, name, true, Guid.NewGuid());

        Result result = category.CheckRequiredFields();

        if (result.IsFailure)
            return Result.Failure<Category>(result.Errors!);

        return Result.Success(category);
    }

    public Result Update(string name, bool? active)
    {
        Name = name;

        if (active is not null)
            Active = (bool)active;

        return CheckRequiredFields();
    }

    protected override Result CheckRequiredFields()
    {
        Result result = CheckRequiredField(string.IsNullOrWhiteSpace(Name), "Category name must be provided");

        return result;
    }
}
