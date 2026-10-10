using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Update;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Markets;

public sealed class UpdateMarketEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ApiRoutes.Markets}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Markets")
            .WithSummary("Update")
            .WithDescription("Atualiza os dados de um mercado")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        [FromBody] BaseRequest<MarketCommandDTO> command,
        [FromServices] ICommandHandler<UpdateMarketCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.HandleAsync(
            new UpdateMarketCommand(externalId, command.Data?.Name, command.Data?.Active),
            cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.NoContent();
    }
}
