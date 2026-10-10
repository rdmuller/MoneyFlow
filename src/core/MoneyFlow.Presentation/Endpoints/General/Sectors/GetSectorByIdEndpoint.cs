using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Application.UseCases.General.Sectors.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Sectors;

public sealed class GetSectorByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiRoutes.Sectors}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN_OR_USER)
            .WithTags("Sectors")
            .WithSummary("Get by id")
            .WithDescription("Retorna todos os dados de um setor")
            .Produces<BaseResponse<SectorQueryDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        IQueryHandler<GetSectorByExternalIdQuery, SectorQueryDTO> handler,
        CancellationToken cancellationToken)
    {
        Result<SectorQueryDTO> result = await handler.HandleAsync(
            new GetSectorByExternalIdQuery(externalId),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(BaseResponse<SectorQueryDTO>.CreateSuccessResponse(result.Value))
            : Results.NoContent();
    }
}
