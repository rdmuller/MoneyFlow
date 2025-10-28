using MoneyFlow.Domain.General.Security;

namespace MoneyFlow.API.Security;

public class HttpContextTokenValue(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string TokenOnRequest()
    {
        var authorization = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString();

        return authorization["Bearer ".Length..].Trim();
    }
}
