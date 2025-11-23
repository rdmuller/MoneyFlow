using FluentValidation;
using MoneyFlow.Application.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands;

public class CategoryCommandDTOValidator : CommonValidator<BaseRequest<CategoryCommandDTO>>
{
    public CategoryCommandDTOValidator()
    {
        RuleFor(x => x.Data).NotNull().WithMessage("Category 'data' must be provided.");
        When(x => x.Data is not null, () =>
        {
            RuleFor(x => x.Data!.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters");
        });
    }
}
