using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Users;
using MoneyFlow.Domain.Common.Repositories.Users;
using MoneyFlow.Domain.Common.Security;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.Common.Users.Queries.GetLoggedUserProfile;

public class GetLoggedUserProfileHandler(IUserReadRepository userQueryRepository, ILoggedUser loggedUser) : IHandler<GetLoggedUserProfileQuery, BaseResponse<GetUserFullQueryDTO>>
{
    private readonly IUserReadRepository _userQueryRepository = userQueryRepository;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<BaseResponse<GetUserFullQueryDTO>> HandleAsync(GetLoggedUserProfileQuery request, CancellationToken cancellationToken = default)
    {
        var userId = await _loggedUser.GetUserIdAsync();
        var user = await _userQueryRepository.GetUserByIdAsync(userId);
        var userDTO = GetUserFullQueryDTO.EntityToDTO(user);

        return BaseResponse<GetUserFullQueryDTO>.CreateSuccessResponse(userDTO);
    }
}
