using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Users;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.Application.UseCases.Common.Users.Commands.Update;
public class UpdateUserProfileCommand : IRequest<BaseResponse<string>>
{
    public UpdateUserProfileCommandDTO? user { get; set; }
}
