using MoneyFlow.Application.UseCases.General.Users.Commands.Validators;
using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Register;

internal class RegisterUserCommandHandler(
    IUserWriteOnlyRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserReadRepository userQueryRepository,
    IPasswordHasher passwordHasher) : ICommandHandler<RegisterUserCommand, string>
{
    private readonly IUserWriteOnlyRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserReadRepository _userQueryRepository = userQueryRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<string>> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        Result<User> user = User.Create(request.Name, new Email(request.Email), request.Password, _passwordHasher);

        if (user.IsFailure)
            return Result.Failure<string>(user.Errors!);

        Result validationResult = await ValidateAsync(user.Value, request.Password);
        if (validationResult.IsFailure)
            return Result.Failure<string>(validationResult.Errors!);

        await _userRepository.CreateAsync(user.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(user.Value.ExternalId!.Value.ToString());
    }

    private async Task<Result> ValidateAsync(User user, string? password = "")
    {
        List<Error> errors = await new UserValidator().ValidateWithErrorsAsync(user);

        if (!string.IsNullOrWhiteSpace(user.Email.Value))
        {
            bool emailExist = await _userQueryRepository.ExistUserWithEmailAsync(user.Email.Value);

            if (emailExist)
                errors.Add(Error.RecordAlreadyExists("E-mail already exists"));
        }

        List<Error> passwordError = await new UserPasswordValidator().ValidateWithErrorsAsync(password);
        if (passwordError.Count > 0)
            errors.AddRange(passwordError);

        if (errors.Count > 0)
            return Result.Failure(errors);

        return Result.Success();
    }
}
