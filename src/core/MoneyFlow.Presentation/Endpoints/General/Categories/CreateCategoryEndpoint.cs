using MoneyFlow.Application.UseCases.General.Categories.Commands.Create;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Categories;

public sealed class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Categories, HandleAsync)
            .RequireAuthorization(Roles.ADMIN)
            .WithTags("Categories")
            .WithSummary("Create")
            .WithDescription("Cria uma nova categoria")
            .Produces<BaseResponse<string>>(StatusCodes.Status201Created)
            .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        ICommandHandler<CreateCategoryCommand, Guid> handler,
        BaseRequest<CreateCategoryCommand> command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await handler.HandleAsync(command.Data, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Results.Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }
}
