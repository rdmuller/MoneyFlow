using MoneyFlow.Application.DTOs.General.Categories;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetByExternalId;

public sealed record GetCategoryByExternalIdQuery(Guid ExternalId) : IRequest<BaseResponse<CategoryQueryDTO>>;