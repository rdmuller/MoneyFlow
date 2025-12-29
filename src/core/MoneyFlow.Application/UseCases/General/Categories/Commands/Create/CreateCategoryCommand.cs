using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Create;

public record CreateCategoryCommand(string name) : IRequest<BaseResponse<string>>
{
    //public CategoryCommandDTO Category { get; set; } = null!;
}
