using MoneyFlow.Application.DTOs.General.Categories;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetByExternalId;

public sealed record GetCategoryByExternalIdQuery(Guid ExternalId) : IQuery<CategoryQueryDTO>;