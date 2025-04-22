using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Users;

namespace MoneyFlow.Application.UseCases.Users.Commands.Register;

public class RegisterUserCommand : IRequest<string>
{
    public RegisterUserCommandDTO? data { get; set; }
}