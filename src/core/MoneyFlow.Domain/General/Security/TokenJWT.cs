namespace MoneyFlow.Domain.General.Security;

public class TokenJwt
{
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; set; }
}
