using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Application.UseCases.General.Categories.Queries.GetAll;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.APIs.Models;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Categories;

public sealed class GetAllCategoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Categories, HandleAsync)
            .RequireAuthorization(Roles.ADMIN_OR_USER)
            .WithTags("Categories")
            .WithSummary("Get list")
            .WithDescription("Retorna lista de categorias")
            .Produces<BaseResponse<IReadOnlyList<CategoryQueryDTO>>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> HandleAsync(
        BoundQueryParams queryParams,
        IQueryHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryQueryDTO>> handler,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<CategoryQueryDTO>> result = await handler.HandleAsync(
            new GetAllCategoriesQuery { Query = queryParams },
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(BaseResponse<IReadOnlyList<CategoryQueryDTO>>.CreatePaginatedResponse(result))
            : Results.NoContent();
    }
}
