using MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Currencies;

public sealed class CreateCurrencyEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Currencies, HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Currencies")
            .WithSummary("Create")
            .WithDescription("Cria uma nova moeda")
            .Produces<BaseResponse<string>>(StatusCodes.Status201Created)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        ICommandHandler<CreateCurrencyCommand, Guid> handler,
        BaseRequest<CreateCurrencyCommand> command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await handler.HandleAsync(command.Data, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }
}
