using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Users;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.Application.UseCases.Users.Commands.Register;

public class RegisterUserCommand : BaseRequest<RegisterUserCommandDTO>, IRequest<BaseResponse<string>>
{
    //public RegisterUserCommandDTO? data { get; set; }
}