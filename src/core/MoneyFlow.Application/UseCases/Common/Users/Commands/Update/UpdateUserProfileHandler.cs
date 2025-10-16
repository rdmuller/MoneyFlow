using Mediator.Abstractions;
using MoneyFlow.Application.UseCases.Common.Users.Commands.Validators;
using MoneyFlow.Common.Communications;
using MoneyFlow.Domain.Common.Entities.Users;
using MoneyFlow.Domain.Common.Repositories.Users;
using MoneyFlow.Domain.Common.Security;

namespace MoneyFlow.Application.UseCases.Common.Users.Commands.Update;
public class UpdateUserProfileHandler(ILoggedUser loggedUser, IUserWriteOnlyRepository userWriteOnlyRepository) : IHandler<UpdateUserProfileCommand, BaseResponse<string>>
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;

    public async Task<BaseResponse<string>> HandleAsync(UpdateUserProfileCommand request, CancellationToken cancellationToken = default)
    {
        await Validate(request.user.DtoToEntity());

        var userId = await _loggedUser.GetUserIdAsync();
    }

    private async Task Validate(User user)
    {
        await new UserValidator().ValidateAndThrowWhenErrorAsync(user);
    }
}