using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Update;

public class UpdateCategoryCommand : IRequest<BaseResponse<string>>
{
    public CategoryCommandDTO? Category { get; set; }
}
