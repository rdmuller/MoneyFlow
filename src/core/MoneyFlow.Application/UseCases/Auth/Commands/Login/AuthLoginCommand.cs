using Mediator.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MoneyFlow.Application.UseCases.Auth.Commands.Login;

public class AuthLoginCommand : IRequest<string>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}