using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Users;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.Application.UseCases.Users.Queries.GetLoggedUserProfile;

public class GetLoggedUserProfileQuery : IRequest<BaseResponse<GetUserFullQueryDTO>>
{
}
