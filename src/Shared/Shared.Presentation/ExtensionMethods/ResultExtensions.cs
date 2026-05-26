namespace Shared.Presentation.ExtensionMethods;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return new BadRequestObjectResult(new BaseResponseError(result.Errors!));
    }
}
