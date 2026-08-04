using MoneyFlow.Application.UseCases.General.Users.Commands.Validators;
using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.ChangePassword;

public class UserChangePasswordCommandHandler(
    ILoggedUser loggedUser,
    IUserWriteOnlyRepository userWriteOnlyRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IRequestHandler<UserChangePasswordCommand, Result>
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(UserChangePasswordCommand request, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(request.NewPassword!);

        long userId = await _loggedUser.GetUserIdAsync();
        User? user = await _userWriteOnlyRepository.GetUserByIdAsync(userId, cancellationToken);

        if (!_passwordHasher.Verify(request.OldPassword!, user!.Password))
            return Result.Failure(Error.ValidationError("Old password does not match"));

        user.ChangePassword(request.NewPassword!, _passwordHasher);

        _userWriteOnlyRepository.Update(user, cancellationToken);

        Console.WriteLine($"User {userId} changed password at {DateTime.UtcNow}");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        //await _domainEvents.DispatchAsync([new UserChangePasswordDomainEvent(user)], cancellationToken);

        return Result.Success();
    }

    private async Task ValidateAsync(string password)
    {
        await new UserPasswordValidator().ValidateAndThrowWhenErrorAsync(password);
    }
}
