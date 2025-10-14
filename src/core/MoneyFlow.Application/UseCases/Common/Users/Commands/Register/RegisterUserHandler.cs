using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Users;
using MoneyFlow.Application.UseCases.Common.Users.Commands.Validators;
using MoneyFlow.Common.Communications;
using MoneyFlow.Common.Exceptions;
using MoneyFlow.Domain.Common.Entities.Users;
using MoneyFlow.Domain.Common.Repositories;
using MoneyFlow.Domain.Common.Repositories.Users;
using MoneyFlow.Domain.Common.Security;

namespace MoneyFlow.Application.UseCases.Common.Users.Commands.Register;

public class RegisterUserHandler(
    IUserWriteOnlyRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserQueryRepository userQueryRepository,
    IPasswordHasher passwordHasher) : IHandler<RegisterUserCommand, BaseResponse<string>>
{
    private readonly IUserWriteOnlyRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserQueryRepository _userQueryRepository = userQueryRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<BaseResponse<string>> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        if (request.User is null)
            throw ErrorOnValidationException.DataNotFound();

        var user = request.User.DtoToEntity();

        await ValidateAsync(user);

        user.Password = _passwordHasher.Hash(user.Password);

        await _userRepository.CreateUserAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();

        return new BaseResponse<string>()
        {
            ObjectId = user.Id,
        };
    }

    private async Task ValidateAsync(User user)
    {
        var errors = await new UserValidator().ValidateWithErrorsAsync(user);

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            var emailExist = await _userQueryRepository.ExistUserWithEmailAsync(user.Email);

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