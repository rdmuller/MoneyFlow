using FluentValidation;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Users;
using MoneyFlow.Common.Communications;
using MoneyFlow.Common.Exceptions;
using MoneyFlow.Domain.Repositories;
using MoneyFlow.Domain.Repositories.Users;

namespace MoneyFlow.Application.UseCases.Users.Commands.Register;

public class RegisterUserHandler(
    IUserWriteOnlyRepository userRepository, 
    IUnitOfWork unitOfWork) : IHandler<RegisterUserCommand, BaseResponse<string>>
{
    private readonly IUserWriteOnlyRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<BaseResponse<string>> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        if (request.User is null)
            throw ErrorOnValidationException.DataNotFound();

        await ValidateAsync(request.User);

        var user = request.User.DtoToEntity();

        await _userRepository.CreateUserAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();

        return new BaseResponse<string>()
        {
            ObjectId = user.Id,
        };
    }

    private async Task ValidateAsync(RegisterUserCommandDTO user)
    {
        var result = await new RegisterUserValidator().ValidateAsync(user);

        if (!result.IsValid)
        {
            var failures = result.Errors
                .Select(e => new BaseError() { ErrorCode = e.ErrorCode, ErrorMessage = e.ErrorMessage })
                .ToList();
            throw new ErrorOnValidationException(failures);
        }


        /*var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }*/

    }
}