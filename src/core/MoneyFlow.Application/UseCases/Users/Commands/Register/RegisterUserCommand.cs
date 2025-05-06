using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Users;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.Application.UseCases.Users.Commands.Register;

public class RegisterUserCommand : IRequest<BaseResponse<string>>
{
    public RegisterUserCommandDTO? User { get; set; }
}