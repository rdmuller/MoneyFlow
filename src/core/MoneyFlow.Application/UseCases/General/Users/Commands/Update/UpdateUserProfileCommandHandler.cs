using MoneyFlow.Application.UseCases.General.Users.Commands.Validators;
using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Update;

public class UpdateUserProfileCommandHandler(ILoggedUser loggedUser, IUserWriteOnlyRepository userWriteOnlyRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserProfileCommand>
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(UpdateUserProfileCommand request, CancellationToken cancellationToken = default)
    {
        Result validationResult = await Validate(User.Create(request.Name, new Email(request.Email)).Value);

        if (validationResult.IsFailure)
            return Result.Failure(validationResult.Errors!);

        long userId = await _loggedUser.GetUserIdAsync();
        User user = await _userWriteOnlyRepository.GetUserByIdAsync(userId, cancellationToken);

        Result result = user.Update(request.Name, new Email(request.Email));
        if (result.IsFailure)
            return Result.Failure(result.Errors!);

        _userWriteOnlyRepository.Update(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Result> Validate(User user)
    {
        List<Error> errors = await new UserValidator().ValidateWithErrorsAsync(user);

        return errors.Count == 0 ? Result.Success() : Result.Failure(errors);
    }
}