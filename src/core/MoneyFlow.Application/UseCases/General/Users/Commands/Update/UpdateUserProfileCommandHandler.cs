using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.UseCases.General.Users.Commands.Validators;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Update;

public class UpdateUserProfileCommandHandler(ILoggedUser loggedUser, IUserWriteOnlyRepository userWriteOnlyRepository, IUnitOfWork unitOfWork) : IHandler<UpdateUserProfileCommand, BaseResponse<string>>
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(UpdateUserProfileCommand request, CancellationToken cancellationToken = default)
    {
        await Validate(User.Create(request.Name, new Email(request.Email)));

        var userId = await _loggedUser.GetUserIdAsync();
        var user = await _userWriteOnlyRepository.GetUserByIdAsync(userId, cancellationToken);

        user.Update(request.Name, new Email(request.Email));

        _userWriteOnlyRepository.Update(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>();
    }

    private async Task Validate(User user)
    {
        await new UserValidator().ValidateAndThrowWhenErrorAsync(user);
    }
}