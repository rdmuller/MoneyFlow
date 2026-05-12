using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Abstractions;
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

    public static Result<Sector> Create(string name, Category category)
    {
        var sector = new Sector(0, name, category.Id, true, Guid.CreateVersion7());

        Result result = sector.CheckRequiredFields();
        if (result.IsFailure)
            return Result.Failure<Sector>(result.Errors!);

        result = sector.CheckForeignKeys(category);
        if (result.IsFailure)
            return Result.Failure<Sector>(result.Errors!);

        return Result.Success(sector);
    }

    public Result Update(string name, Category category, bool active = true)
    {
        Name = name;
        Active = active;
        CategoryId = category is not null ? category.Id : 0;

        Result result = CheckRequiredFields();
        if (result.IsFailure)
            return result;

        result = CheckForeignKeys(category!);
        if (result.IsFailure)
            return result;

        return Result.Success();
    }

    private Result CheckForeignKeys(Category category)
    {
        return CheckRule(new CategoryMustBeActiveBusinessRule(category));
    }

    protected override Result CheckRequiredFields()
    {
        Result result = CheckRequiredField(string.IsNullOrWhiteSpace(Name), "Sector name is required");
        if (result.IsFailure)
            return result;

        return CheckRequiredField(CategoryId.Equals(0), "Category is required");
    }
}