using Mediator.Abstractions;
using MoneyFlow.Common.Communications;
using MoneyFlow.Domain.Common.Repositories.Users;
using MoneyFlow.Domain.Common.Security;

namespace MoneyFlow.Application.UseCases.Common.Users.Commands.Update;
public class UpdateUserProfileHandler(ILoggedUser loggedUser, IUserWriteOnlyRepository userWriteOnlyRepository) : IHandler<UpdateUserProfileCommand, BaseResponse<string>>
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;

    public async Task<BaseResponse<string>> HandleAsync(UpdateUserProfileCommand request, CancellationToken cancellationToken = default)
    {
        var userId = await _loggedUser.GetUserIdAsync();

        throw new NotImplementedException();
    }
}
