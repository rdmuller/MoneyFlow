using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Application.UseCases.General.Categories.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Categories;

public sealed class GetCategoryByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiRoutes.Categories}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN_OR_USER)
            .WithTags("Categories")
            .WithSummary("Get by id")
            .WithDescription("Retorna todos os dados de uma categoria")
            .Produces<BaseResponse<CategoryQueryDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        IQueryHandler<GetCategoryByExternalIdQuery, CategoryQueryDTO> handler,
        CancellationToken cancellationToken)
    {
        Result<CategoryQueryDTO> result = await handler.HandleAsync(
            new GetCategoryByExternalIdQuery(externalId),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(BaseResponse<CategoryQueryDTO>.CreateSuccessResponse(result.Value))
            : Results.NoContent();
    }
}
