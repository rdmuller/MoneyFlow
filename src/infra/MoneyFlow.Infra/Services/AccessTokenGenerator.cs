using Microsoft.IdentityModel.Tokens;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using SharedKernel.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyFlow.Infra.Services;

public class AccessTokenGenerator(
    uint expirationTimeInMinutes,
    string signingKey,
    IDateTimeProvider dateTimeProvider) : IAccessTokenGenerator
{
    private readonly uint _expirationTimeInMinutes = expirationTimeInMinutes;
    private readonly string _signinKey = signingKey;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public TokenJwt GenerateAccessToken(User user)
    {
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Email, user.Email.Value),
            new Claim(ClaimTypes.Sid, user.ExternalId.ToString()!),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, user.Name),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = _dateTimeProvider.UtcNow.AddMinutes(_expirationTimeInMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        var token = new TokenJwt
        {
            Token = tokenHandler.WriteToken(securityToken),
            ExpiresAt = tokenDescriptor.Expires!.Value,
        };

        return token;
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signinKey);

        return new SymmetricSecurityKey(key);
    }
}