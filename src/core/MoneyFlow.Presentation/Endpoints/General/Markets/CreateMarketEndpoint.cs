using MoneyFlow.Application.UseCases.General.Markets.Commands.Create;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Markets;

public sealed class CreateMarketEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Markets, HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Markets")
            .WithSummary("Create")
            .WithDescription("Cria um novo mercado")
            .Produces<BaseResponse<string>>(StatusCodes.Status201Created)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        ICommandHandler<CreateMarketCommand, Guid> handler,
        BaseRequest<CreateMarketCommand> command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await handler.HandleAsync(command.Data, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }
}
