using Mediator.Abstractions;
using MoneyFlow.Common.Exceptions;
using MoneyFlow.Domain.Entities;
using MoneyFlow.Domain.Repositories.Users;

namespace MoneyFlow.Application.UseCases.Auth.Commands.Login;

public class AuthLoginHandler(IUserQueryRepository userQueryRepository) : IHandler<AuthLoginCommand, string>
{
    private readonly IUserQueryRepository _userQueryRepository = userQueryRepository;

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

        return user;
    }
}
