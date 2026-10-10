using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Update;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Currencies;

public sealed class UpdateCurrencyEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ApiRoutes.Currencies}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Currencies")
            .WithSummary("Update")
            .WithDescription("Atualiza os dados de uma moeda")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        [FromBody] BaseRequest<CurrencyCommandDTO> command,
        [FromServices] ICommandHandler<UpdateCurrencyCommand> handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.HandleAsync(
            new UpdateCurrencyCommand(externalId, command.Data?.Name, command.Data?.Symbol, command.Data?.Active),
            cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.NoContent();
    }
}
