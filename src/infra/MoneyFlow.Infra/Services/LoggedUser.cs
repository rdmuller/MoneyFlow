using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Security;
using MoneyFlow.Infra.DataAccess;
using System.Security.Claims;

namespace MoneyFlow.Infra.Services;

public class LoggedUser(ITokenProvider tokenProvider, ApplicationDbContext dbContext) : ILoggedUser
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<long> GetUserIdAsync()
    {
        string token = _tokenProvider.TokenOnRequest();
        System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler tokenHandler = new();
        System.IdentityModel.Tokens.Jwt.JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

        string userExternalId = jwtToken.Claims.First(jwtToken => jwtToken.Type == ClaimTypes.Sid).Value;

        return await _dbContext
            .Users
            .AsNoTracking()
            .Where(u => u.ExternalId == Guid.Parse(userExternalId))
            .Select(u => u.Id)
            .FirstAsync();
    }
}
