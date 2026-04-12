using MoneyFlow.Application.DTOs.General.Users;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetLoggedUserProfile;

public class GetLoggedUserProfileQuery : IRequest<BaseResponse<GetUserFullQueryDTO>>
{
}
