using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Delete;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Currencies;

public sealed class DeleteCurrencyEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ApiRoutes.Currencies}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Currencies")
            .WithSummary("Delete")
            .WithDescription("Exclui uma moeda")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        [FromServices] ICommandHandler<DeleteCurrencyCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.HandleAsync(new DeleteCurrencyCommand(externalId), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.NoContent();
    }
}
