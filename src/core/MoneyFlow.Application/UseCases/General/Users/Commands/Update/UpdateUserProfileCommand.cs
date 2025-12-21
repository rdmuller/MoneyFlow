using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Users;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Update;

public class UpdateUserProfileCommand : IRequest<BaseResponse<string>>
{
    public UpdateUserProfileCommandDTO? user { get; set; }
}
