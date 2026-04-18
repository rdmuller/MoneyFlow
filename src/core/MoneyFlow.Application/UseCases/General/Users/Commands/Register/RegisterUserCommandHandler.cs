using MoneyFlow.Application.UseCases.General.Users.Commands.Validators;
using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Register;

internal class RegisterUserCommandHandler(
    IUserWriteOnlyRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserReadRepository userQueryRepository,
    IPasswordHasher passwordHasher) : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserWriteOnlyRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserReadRepository _userQueryRepository = userQueryRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<Guid>> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = User.Create(request.Name, new Email(request.Email), request.Password, _passwordHasher);

        if (user.IsFailure)
            return Result.Failure<Guid>(user.Errors!);

        var validationResult = await ValidateAsync(user.Value);
        if (validationResult.IsFailure)
            return Result.Failure<Guid>(validationResult.Errors!);

        await _userRepository.CreateAsync(user.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Success(user.Value.ExternalId!.Value);
    }

    private async Task<Result> ValidateAsync(User user)
    {
        var errors = await new UserValidator().ValidateWithErrorsAsync(user);

        if (!string.IsNullOrWhiteSpace(user.Email.Value))
        {
            var emailExist = await _userQueryRepository.ExistUserWithEmailAsync(user.Email.Value);

            if (emailExist)
                errors.Add(Error.RecordAlreadyExists("E-mail already exists"));
        }

        var passwordError = await new UserPasswordValidator().ValidateWithErrorsAsync(user.Password);
        if (passwordError.Count > 0)
            errors.AddRange(passwordError);

        if (errors.Count > 0)
            return Result.Failure(errors);

        return Result.Success();
    }
}