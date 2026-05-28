using MoneyFlow.Domain.General.Security;

namespace MoneyFlow.Presentation.Security;

public class HttpContextTokenValue(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string TokenOnRequest()
    {
        string authorization = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString();

        return authorization["Bearer ".Length..].Trim();
    }
}
