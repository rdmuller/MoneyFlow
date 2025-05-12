namespace MoneyFlow.Application.DTOs.Auth;
public class TokenDTO
{
    public string Token { get; set; }

    public int ExpiresAt { get; set; }
}
