using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace MoneyFlow.Application.UseCases.Auth.Commands.Login;

public class AuthLoginCommand : IRequest<TokenDTO>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}