using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Sectors;

public sealed class UpdateSectorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ApiRoutes.Sectors}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Sectors")
            .WithSummary("Update")
            .WithDescription("Atualiza os dados de um setor")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        [FromBody] BaseRequest<SectorCommandDTO> command,
        [FromServices] ICommandHandler<UpdateSectorCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.HandleAsync(
            new UpdateSectorCommand(externalId, command.Data?.Name, command.Data?.CategoryExternalId, command.Data?.Active),
            cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.NoContent();
    }
}
