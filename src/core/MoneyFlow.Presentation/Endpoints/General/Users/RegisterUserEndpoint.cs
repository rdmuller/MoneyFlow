using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.UseCases.General.Users.Commands.Register;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Users;

public sealed class RegisterUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Users, HandleAsync)
            .AllowAnonymous()
            .WithTags("Users")
            .WithSummary("Register new user")
            .WithDescription("Cadastra um novo usuário no sistema")
            .Produces<BaseResponse<string>>(StatusCodes.Status201Created)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        [FromServices] ICommandHandler<RegisterUserCommand, string> handler,
        [FromBody] BaseRequest<RegisterUserCommand> command,
        CancellationToken cancellationToken)
    {
        Result<string> result = await handler.HandleAsync(command.Data, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }
}
