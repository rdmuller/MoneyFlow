using MoneyFlow.Application.DTOs.General.Users;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetLoggedUserProfile;

internal class GetLoggedUserProfileQueryHandler(IUserReadRepository userQueryRepository, ILoggedUser loggedUser)
    : IQueryHandler<GetLoggedUserProfileQuery, GetUserFullQueryDTO>
{
    private readonly IUserReadRepository _userQueryRepository = userQueryRepository;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<Result<GetUserFullQueryDTO>> HandleAsync(GetLoggedUserProfileQuery request, CancellationToken cancellationToken = default)
    {
        var userId = await _loggedUser.GetUserIdAsync();
        var user = await _userQueryRepository.GetByIdAsync(userId);
        var userDTO = GetUserFullQueryDTO.EntityToDTO(user);

        return Result<GetUserFullQueryDTO>.Create(userDTO);
    }
}
