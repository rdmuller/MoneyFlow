using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetById;
public class GetCategoryByIdQuery : IRequest<BaseResponse<CategoryQueryDTO>>
{
    public long Id { get; set; }
}
