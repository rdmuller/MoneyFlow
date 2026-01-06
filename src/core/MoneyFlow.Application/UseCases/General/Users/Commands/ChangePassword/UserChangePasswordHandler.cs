using Mediator.Abstractions;
using MoneyFlow.Application.Common.Events;
using MoneyFlow.Application.UseCases.General.Users.Commands.Validators;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.ChangePassword;

public class UserChangePasswordHandler(
    ILoggedUser loggedUser,
    IUserWriteOnlyRepository userWriteOnlyRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IDomainEventsDispatcher domainEvents) : IHandler<UserChangePasswordCommand, BaseResponse<string>>
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDomainEventsDispatcher _domainEvents = domainEvents;

    public async Task<BaseResponse<string>> HandleAsync(UserChangePasswordCommand request, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(request.NewPassword!);

        var userId = await _loggedUser.GetUserIdAsync();
        var user = await _userWriteOnlyRepository.GetUserByIdAsync(userId);

        if (!_passwordHasher.Verify(request.OldPassword!, user.Password))
            throw AuthorizationException.InvalidData("Old password does not match");

        user.Password = _passwordHasher.Hash(request.NewPassword!);
        _userWriteOnlyRepository.Update(user, cancellationToken);

        Console.WriteLine($"User {userId} changed password at {DateTime.UtcNow}");

        await _unitOfWork.CommitAsync(cancellationToken);
        await _domainEvents.DispatchAsync([new UserChangePasswordDomainEvent(user)], cancellationToken);

        return new BaseResponse<string>();
    }

    private async Task ValidateAsync(string password)
    {
        await new UserPasswordValidator().ValidateAndThrowWhenErrorAsync(password);
    }
}
