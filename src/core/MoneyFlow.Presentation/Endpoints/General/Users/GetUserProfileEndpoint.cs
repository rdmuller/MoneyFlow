using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Users;
using MoneyFlow.Application.UseCases.General.Users.Queries.GetLoggedUserProfile;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Users;

public sealed class GetUserProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiRoutes.Users}/profile", HandleAsync)
            .RequireAuthorization()
            .WithTags("Users")
            .WithSummary("Get user profile")
            .WithDescription("Retorna os dados completos do usuário autenticado")
            .Produces<BaseResponse<GetUserFullQueryDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> HandleAsync(
        [FromServices] IQueryHandler<GetLoggedUserProfileQuery, GetUserFullQueryDTO> handler,
        CancellationToken cancellationToken)
    {
        Result<GetUserFullQueryDTO> result = await handler.HandleAsync(new GetLoggedUserProfileQuery(), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(BaseResponse<GetUserFullQueryDTO>.CreateSuccessResponse(result.Value))
            : Results.NoContent();
    }
}
