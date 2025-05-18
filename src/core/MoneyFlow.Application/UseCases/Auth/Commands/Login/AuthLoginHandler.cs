using Mediator.Abstractions;
using MoneyFlow.Common.Exceptions;
using MoneyFlow.Domain.Entities;
using MoneyFlow.Domain.Repositories.Users;
using MoneyFlow.Domain.Security;

namespace MoneyFlow.Application.UseCases.Auth.Commands.Login;

public class AuthLoginHandler(
    IUserQueryRepository userQueryRepository,
    IPasswordHasher passwordHasher) : IHandler<AuthLoginCommand, string>
{
    private readonly IUserQueryRepository _userQueryRepository = userQueryRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<string> HandleAsync(AuthLoginCommand request, CancellationToken cancellationToken = default)
    {
        var user = await ValidateLogin(request);

        var token = "token";
        return token;
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
