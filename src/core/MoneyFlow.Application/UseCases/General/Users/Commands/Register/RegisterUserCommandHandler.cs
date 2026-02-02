using Mediator.Abstractions;
using MoneyFlow.Application.UseCases.General.Users.Commands.Validators;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Register;

public class RegisterUserCommandHandler(
    IUserWriteOnlyRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserReadRepository userQueryRepository,
    IPasswordHasher passwordHasher) : IHandler<RegisterUserCommand, BaseResponse<string>>
{
    private readonly IUserWriteOnlyRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserReadRepository _userQueryRepository = userQueryRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<BaseResponse<string>> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = User.Create(request.Name, new Email(request.Email));

        await ValidateAsync(user);

        user.SetPassword(user.Password, _passwordHasher);

        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();

        return new BaseResponse<string>()
        {
            ObjectId = user.ExternalId,
        };
    }

    private async Task ValidateAsync(User user)
    {
        var errors = await new UserValidator().ValidateWithErrorsAsync(user);

        if (!string.IsNullOrWhiteSpace(user.Email.Value))
        {
            var emailExist = await _userQueryRepository.ExistUserWithEmailAsync(user.Email.Value);

            if (emailExist)
                errors.Add(BaseError.RecordAlreadyExists("E-mail already exists"));
        }

        var passwordError = await new UserPasswordValidator().ValidateWithErrorsAsync(user.Password);
        if (passwordError.Count > 0)
            errors.AddRange(passwordError);

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }
}