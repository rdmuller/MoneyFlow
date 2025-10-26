using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Users;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.Common.Users.Queries.GetLoggedUserProfile;

public class GetLoggedUserProfileQuery : IRequest<BaseResponse<GetUserFullQueryDTO>>
{
}
