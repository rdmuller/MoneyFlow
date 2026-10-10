using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Application.UseCases.General.Currencies.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Currencies;

public sealed class GetCurrencyByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiRoutes.Currencies}/{{externalId:guid}}", HandleAsync)
            .RequireAuthorization(Roles.ADMIN_OR_USER)
            .WithTags("Currencies")
            .WithSummary("Get by id")
            .WithDescription("Retorna todos os dados de uma moeda")
            .Produces<BaseResponse<CurrencyQueryDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> HandleAsync(
        Guid externalId,
        IQueryHandler<GetCurrencyByExternalIdQuery, CurrencyQueryDTO> handler,
        CancellationToken cancellationToken)
    {
        Result<CurrencyQueryDTO> result = await handler.HandleAsync(
            new GetCurrencyByExternalIdQuery(externalId),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(BaseResponse<CurrencyQueryDTO>.CreateSuccessResponse(result.Value))
            : Results.NoContent();
    }
}
