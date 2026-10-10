using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.APIs.Models;
using Shared.Presentation.Communications;
using Shared.Presentation.Endpoints;

namespace MoneyFlow.Presentation.Endpoints.General.Currencies;

public sealed class GetAllCurrenciesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Currencies, HandleAsync)
            .RequireAuthorization(Roles.ADMIN_OR_USER)
            .WithTags("Currencies")
            .WithSummary("Get list")
            .WithDescription("Retorna lista de moedas")
            .Produces<BaseResponse<IReadOnlyList<CurrencyQueryDTO>>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> HandleAsync(
        BoundQueryParams queryParams,
        IQueryHandler<GetAllCurrenciesQuery, IReadOnlyList<CurrencyQueryDTO>> handler,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<CurrencyQueryDTO>> result = await handler.HandleAsync(
            new GetAllCurrenciesQuery { Query = queryParams },
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(BaseResponse<IReadOnlyList<CurrencyQueryDTO>>.CreatePaginatedResponse(result))
            : Results.NoContent();
    }
}
