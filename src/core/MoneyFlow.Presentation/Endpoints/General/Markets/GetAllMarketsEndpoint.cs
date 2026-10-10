using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.APIs.Models;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Markets;

public sealed class GetAllMarketsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Markets, HandleAsync)
            .RequireAuthorization(Roles.ADMIN_OR_USER)
            .WithTags("Markets")
            .WithSummary("Get list")
            .WithDescription("Retorna lista de mercados")
            .Produces<BaseResponse<IReadOnlyList<MarketQueryDTO>>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> HandleAsync(
        BoundQueryParams queryParams,
        IQueryHandler<GetAllMarketsQuery, IReadOnlyList<MarketQueryDTO>> handler,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<MarketQueryDTO>> result = await handler.HandleAsync(
            new GetAllMarketsQuery { Query = queryParams },
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(BaseResponse<IReadOnlyList<MarketQueryDTO>>.CreatePaginatedResponse(result))
            : Results.NoContent();
    }
}
