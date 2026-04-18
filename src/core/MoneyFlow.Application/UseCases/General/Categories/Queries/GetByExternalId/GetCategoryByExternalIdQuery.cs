using MoneyFlow.Application.DTOs.General.Categories;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetByExternalId;

public sealed record GetCategoryByExternalIdQuery(Guid ExternalId) : IQuery<CategoryQueryDTO>;