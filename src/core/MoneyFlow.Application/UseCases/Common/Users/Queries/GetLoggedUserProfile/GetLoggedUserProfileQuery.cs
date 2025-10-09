using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Users;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.Application.UseCases.Common.Users.Queries.GetLoggedUserProfile;

public class GetLoggedUserProfileQuery : IRequest<BaseResponse<GetUserFullQueryDTO>>
{
}
