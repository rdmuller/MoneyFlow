using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Delete;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Sectors;

public sealed class DeleteSectorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ApiRoutes.Sectors}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Sectors")
            .WithSummary("Delete")
            .WithDescription("Exclui um setor")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        [FromServices] ICommandHandler<DeleteSectorCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.HandleAsync(new DeleteSectorCommand(externalId), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.NoContent();
    }
}
