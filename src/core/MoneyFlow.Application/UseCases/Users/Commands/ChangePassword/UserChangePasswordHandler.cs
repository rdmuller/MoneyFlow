using Mediator.Abstractions;
using MoneyFlow.Common.Communications;
using MoneyFlow.Common.Exceptions;
using MoneyFlow.Domain.Repositories;
using MoneyFlow.Domain.Repositories.Users;
using MoneyFlow.Domain.Security;

namespace MoneyFlow.Application.UseCases.Users.Commands.ChangePassword;

public class UserChangePasswordHandler(
    ILoggedUser loggedUser,
    IUserWriteOnlyRepository userWriteOnlyRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IHandler<UserChangePasswordCommand, BaseResponse<string>>
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(UserChangePasswordCommand request, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(request.NewPassword!);

        var userId = await _loggedUser.GetUserIdAsync();
        var user = await _userWriteOnlyRepository.GetUserByIdAsync(userId);

        if (!_passwordHasher.Verify(request.OldPassword!, user.Password))
            throw new ErrorOnValidationException(BaseError.Unauthorized("Old password does not match"));

        user.Password = _passwordHasher.Hash(request.NewPassword!);
        _userWriteOnlyRepository.UpdateUser(user, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>();
    }

    private async Task ValidateAsync(string password)
    {
        await new UserChangePasswordValidator().ValidateAndThrowWhenErrorAsync(password);
    }
}
