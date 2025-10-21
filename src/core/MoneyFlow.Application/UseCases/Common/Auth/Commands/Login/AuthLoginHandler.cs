using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Auth;
using MoneyFlow.Common.Exceptions;
using MoneyFlow.Domain.Common.Entities.Users;
using MoneyFlow.Domain.Common.Repositories.Users;
using MoneyFlow.Domain.Common.Security;

namespace MoneyFlow.Application.UseCases.Common.Auth.Commands.Login;

public class AuthLoginHandler(
    IUserReadRepository userQueryRepository,
    IPasswordHasher passwordHasher, 
    IAccessTokenGenerator accessTokenGenerator) : IHandler<AuthLoginCommand, TokenDTO>
{
    private readonly IUserReadRepository _userQueryRepository = userQueryRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

    public async Task<TokenDTO> HandleAsync(AuthLoginCommand request, CancellationToken cancellationToken = default)
    {
        var user = await ValidateLogin(request);

        var token = _accessTokenGenerator.GenerateAccessToken(user);
        return new TokenDTO
        {
            Token = token.Token,
            ExpiresAt = token.ExpiresAt
        };
    }

    private async Task<User> ValidateLogin(AuthLoginCommand request)
    {
        var errors = await new AuthLoginValidator().ValidateWithErrorsAsync(request);

        if (errors.Count > 0)
            throw new AuthorizationException(errors);

        var user = await _userQueryRepository.GetUserByEmailAsync(request.Email);

        if (user is null)
            throw AuthorizationException.InvalidData("Invalid e-mail");

        if (!_passwordHasher.Verify(request.Password, user.Password))
            throw AuthorizationException.InvalidData("Invalid password");

        return user;
    }
}
