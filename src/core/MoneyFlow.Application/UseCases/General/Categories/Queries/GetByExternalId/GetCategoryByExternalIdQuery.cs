using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetByExternalId;

public sealed record GetSectorByExternalIdQuery(Guid ExternalId) : IRequest<BaseResponse<CategoryQueryDTO>>;