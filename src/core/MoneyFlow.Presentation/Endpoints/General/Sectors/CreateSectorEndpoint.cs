using MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Sectors;

public sealed class CreateSectorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Sectors, HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Sectors")
            .WithSummary("Create")
            .WithDescription("Cria um novo setor")
            .Produces<BaseResponse<string>>(StatusCodes.Status201Created)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        ICommandHandler<CreateSectorCommand, Guid> handler,
        BaseRequest<CreateSectorCommand> command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await handler.HandleAsync(command.Data, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }
}
