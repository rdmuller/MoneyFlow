using MoneyFlow.Application.DTOs.General.Categories;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetAll;

public sealed class GetAllCategoriesQuery : QueryList<CategoryQueryDTO>;