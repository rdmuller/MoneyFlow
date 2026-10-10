using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Markets;

public sealed class GetMarketByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiRoutes.Markets}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN_OR_USER)
            .WithTags("Markets")
            .WithSummary("Get by id")
            .WithDescription("Retorna todos os dados de um mercado")
            .Produces<BaseResponse<MarketQueryDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        IQueryHandler<GetMarketByExternalIdQuery, MarketQueryDTO> handler,
        CancellationToken cancellationToken)
    {
        Result<MarketQueryDTO> result = await handler.HandleAsync(
            new GetMarketByExternalIdQuery(externalId),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(BaseResponse<MarketQueryDTO>.CreateSuccessResponse(result.Value))
            : Results.NoContent();
    }
}
