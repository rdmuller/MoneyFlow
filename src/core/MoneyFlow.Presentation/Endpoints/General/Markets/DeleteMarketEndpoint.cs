using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Markets;

public sealed class DeleteMarketEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ApiRoutes.Markets}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Markets")
            .WithSummary("Delete")
            .WithDescription("Exclui um mercado")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        [FromServices] ICommandHandler<DeleteMarketCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.HandleAsync(new DeleteMarketCommand(externalId), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.NoContent();
    }
}
