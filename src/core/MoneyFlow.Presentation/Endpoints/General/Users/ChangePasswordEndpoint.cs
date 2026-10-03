using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.UseCases.General.Users.Commands.ChangePassword;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Users;

public sealed class ChangePasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ApiRoutes.Users}/change-password", HandleAsync)
            .RequireAuthorization()
            .WithTags("Users")
            .WithSummary("Change user password")
            .WithDescription("Atualiza a senha do usuário autenticado")
            .Produces<BaseResponse<string>>(StatusCodes.Status200OK)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        [FromServices] ICommandHandler<UserChangePasswordCommand> handler,
        [FromBody] BaseRequest<UserChangePasswordCommand> command,
        CancellationToken cancellationToken)
    {
        Result result = await handler.HandleAsync(command.Data, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.Ok(BaseResponse<string>.CreateSuccessResponse("Password changed successfully"));
    }
}
