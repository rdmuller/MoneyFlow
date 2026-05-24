using MoneyFlow.Application.DTOs.General.Categories;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetAll;

public sealed class GetAllCategoriesQuery : QueryList<CategoryQueryDTO>;