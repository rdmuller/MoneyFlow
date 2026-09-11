using Microsoft.AspNetCore.Http;
using Shared.Domain;

namespace Shared.Presentation.Results;

internal static class ApiResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException();

        return Microsoft.AspNetCore.Http.Results.Problem();
    }
}
