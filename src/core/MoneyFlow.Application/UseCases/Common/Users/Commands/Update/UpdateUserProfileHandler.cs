using Mediator.Abstractions;
using MoneyFlow.Application.UseCases.Common.Users.Commands.Validators;
using MoneyFlow.Domain.Common.Entities.Users;
using MoneyFlow.Domain.Common.Repositories;
using MoneyFlow.Domain.Common.Repositories.Users;
using MoneyFlow.Domain.Common.Security;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.Common.Users.Commands.Update;
public class UpdateUserProfileHandler(ILoggedUser loggedUser, IUserWriteOnlyRepository userWriteOnlyRepository, IUnitOfWork unitOfWork) : IHandler<UpdateUserProfileCommand, BaseResponse<string>>
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(UpdateUserProfileCommand request, CancellationToken cancellationToken = default)
    {
        await Validate(request.user.DtoToEntity());

        var userId = await _loggedUser.GetUserIdAsync();
        var user = await _userWriteOnlyRepository.GetUserByIdAsync(userId, cancellationToken);

        if (request.user.Name is not null)
            user.Name = request.user.Name;

        if (request.user.Email is not null)
            user.Email = request.user.Email;

        _userWriteOnlyRepository.UpdateUser(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>();
    }

    private async Task Validate(User user)
    {
        await new UserValidator().ValidateAndThrowWhenErrorAsync(user);
    }
}