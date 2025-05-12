using Mediator.Abstractions;

namespace MoneyFlow.Application.UseCases.Auth.Commands.Login;

public class AuthLoginHandler : IHandler<AuthLoginCommand, string>
{
    public async Task<string> HandleAsync(AuthLoginCommand request, CancellationToken cancellationToken = default)
    {
        var token = "token";
        return token;
    }
}
