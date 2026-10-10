using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Application.UseCases.General.Sectors.Queries.GetAll;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.APIs.Models;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Sectors;

public sealed class GetAllSectorsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Sectors, HandleAsync)
            .RequireAuthorization(Roles.ADMIN_OR_USER)
            .WithTags("Sectors")
            .WithSummary("Get list")
            .WithDescription("Retorna lista de setores")
            .Produces<BaseResponse<IReadOnlyList<SectorQueryDTO>>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> HandleAsync(
        BoundQueryParams queryParams,
        IQueryHandler<GetAllSectorsQuery, IReadOnlyList<SectorQueryDTO>> handler,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<SectorQueryDTO>> result = await handler.HandleAsync(
            new GetAllSectorsQuery { Query = queryParams },
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(BaseResponse<IReadOnlyList<SectorQueryDTO>>.CreatePaginatedResponse(result))
            : Results.NoContent();
    }
}
