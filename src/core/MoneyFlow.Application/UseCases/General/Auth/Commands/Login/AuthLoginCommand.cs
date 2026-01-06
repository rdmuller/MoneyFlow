using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Auth;

namespace MoneyFlow.Application.UseCases.General.Auth.Commands.Login;

public class AuthLoginCommand : IRequest<TokenDTO>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}