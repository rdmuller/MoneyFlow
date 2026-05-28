using Microsoft.AspNetCore.Http;
using Shared.Domain;

namespace Shared.Presentation.ApiResult;

internal static class ApiResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException();

        return Results.Problem();
    }
}
