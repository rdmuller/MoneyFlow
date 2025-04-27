using Mediator.Abstractions;
using MoneyFlow.Common.Communications;
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
        var user = request.Data!.DtoToEntity();

        await _userRepository.CreateUserAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();

        return new BaseResponse<string>()
        {
            ObjectId = user.Id,
        };
    }
}