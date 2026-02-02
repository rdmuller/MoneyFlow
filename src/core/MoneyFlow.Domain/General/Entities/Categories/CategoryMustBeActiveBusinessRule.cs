using SharedKernel.BusinessRules;
using SharedKernel.Communications;

namespace MoneyFlow.Domain.General.Entities.Categories;

public class CategoryMustBeActiveBusinessRule(Category category) : IBusinessRule
{
    private readonly Category _category = category;

    public BaseError? Error => BaseError.InactiveForeignKey("Category must be active");

    public bool IsBroken() => !_category.Active;
}
