using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.Entities;
using MoneyFlow.Domain.Security;
using MoneyFlow.Infra.DataAccess;

namespace MoneyFlow.Infra.Services;

public class LoggedUser(ITokenProvider tokenProvider, ApplicationDbContext dbContext) : ILoggedUser
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<long> GetUserIdAsync()
    {
        var token = _tokenProvider.TokenOnRequest();
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var userExternalId = jwtToken.Claims.First(jwtToken => jwtToken.Type == ClaimTypes.Sid).Value;

        return await _dbContext
            .Users
            .AsNoTracking()
            .Where(u => u.ExternalId == Guid.Parse(userExternalId))
            .Select(u => u.Id)
            .FirstAsync();
    }
}
