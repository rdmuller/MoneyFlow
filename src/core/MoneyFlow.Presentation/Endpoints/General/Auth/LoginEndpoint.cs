using MoneyFlow.Application.DTOs.General.Auth;
using MoneyFlow.Application.UseCases.General.Auth.Commands.Login;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Auth;

public sealed class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Auth, HandleAsync)
            .AllowAnonymous()
            .WithTags("Auth")
            .WithSummary("User login")
            .WithDescription("Autentica o usuário e retorna o token de acesso")
            .Produces<TokenDTO>(StatusCodes.Status200OK)
            .Produces<IReadOnlyList<string>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> HandleAsync(
        ICommandHandler<AuthLoginCommand, TokenDTO> handler,
        AuthLoginCommand command,
        CancellationToken cancellationToken)
    {
        Result<TokenDTO> result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Errors);

        return Results.Ok(result.Value);
    }
}
