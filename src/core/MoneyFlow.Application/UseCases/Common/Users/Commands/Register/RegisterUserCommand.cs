using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Users;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.Application.UseCases.Common.Users.Commands.Register;

public class RegisterUserCommand : IRequest<BaseResponse<string>>
{
    public RegisterUserCommandDTO? User { get; set; }
}