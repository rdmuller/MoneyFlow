using MoneyFlow.Application.DTOs.General.Users;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetLoggedUserProfile;

internal sealed class GetLoggedUserProfileQueryHandler(IUserReadRepository userQueryRepository, ILoggedUser loggedUser)
    : IQueryHandler<GetLoggedUserProfileQuery, GetUserFullQueryDTO>
{
    private readonly IUserReadRepository _userQueryRepository = userQueryRepository;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<Result<GetUserFullQueryDTO>> HandleAsync(GetLoggedUserProfileQuery request, CancellationToken cancellationToken = default)
    {
        long userId = await _loggedUser.GetUserIdAsync();
        User? user = await _userQueryRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return (Result<GetUserFullQueryDTO>)Result<GetUserFullQueryDTO>.Failure(Error.RecordNotFound("Profile cannot be accessed"));

        return Result.Create(GetUserFullQueryDTO.EntityToDTO(user));
    }
}
