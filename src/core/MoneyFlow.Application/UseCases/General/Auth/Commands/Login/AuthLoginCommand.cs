using MoneyFlow.Application.DTOs.General.Auth;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Auth.Commands.Login;

public class AuthLoginCommand : ICommand<TokenDTO>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}