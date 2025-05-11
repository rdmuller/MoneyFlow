using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Users;
using MoneyFlow.Common.Communications;
using MoneyFlow.Common.Exceptions;
using MoneyFlow.Domain.Repositories;
using MoneyFlow.Domain.Repositories.Users;
using MoneyFlow.Domain.Security;

namespace MoneyFlow.Application.UseCases.Users.Commands.Register;

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

        await ValidateAsync(request.User);

        var user = request.User.DtoToEntity();

        user.Password = _passwordHasher.Hash(user.Password);

        await _userRepository.CreateUserAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();

        return new BaseResponse<string>()
        {
            ObjectId = user.Id,
        };
    }

    private async Task ValidateAsync(RegisterUserCommandDTO user)
    {
        var errors = await new RegisterUserValidator().ValidateWithErrorsAsync(user);

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            var emailExist = await _userQueryRepository.ExistUserWithEmailAsync(user.Email);

            if (emailExist)
                errors.Add(BaseError.RecordAlreadyExists("E-mail already exists"));
        }

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }
}