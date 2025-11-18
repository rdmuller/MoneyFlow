using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries;
public class GetCategoryByIdQuery : IRequest<CategoryQueryDTO>
{
    public long Id { get; set; }
}
