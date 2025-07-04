﻿using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Users;
using MoneyFlow.Common.Communications;
using MoneyFlow.Domain.Repositories.Users;
using MoneyFlow.Domain.Security;

namespace MoneyFlow.Application.UseCases.Users.Queries.GetLoggedUserProfile;

public class GetLoggedUserProfileHandler(IUserQueryRepository userQueryRepository, ILoggedUser loggedUser) : IHandler<GetLoggedUserProfileQuery, BaseResponse<GetUserFullQueryDTO>>
{
    private readonly IUserQueryRepository _userQueryRepository = userQueryRepository;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<BaseResponse<GetUserFullQueryDTO>> HandleAsync(GetLoggedUserProfileQuery request, CancellationToken cancellationToken = default)
    {
        var userId = await _loggedUser.GetUserIdAsync();
        var user = await _userQueryRepository.GetUserByIdAsync(userId);
        var userDTO = GetUserFullQueryDTO.EntityToDTO(user);

        return BaseResponse<GetUserFullQueryDTO>.CreateSuccessResponse(userDTO);
    }
}
