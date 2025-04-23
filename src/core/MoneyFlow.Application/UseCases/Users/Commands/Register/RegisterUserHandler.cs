using Mediator.Abstractions;
using MoneyFlow.Domain.Repositories;
using MoneyFlow.Domain.Repositories.Users;

namespace MoneyFlow.Application.UseCases.Users.Commands.Register;

public class RegisterUserHandler(
    IUserWriteOnlyRepository userRepository, 
    IUnitOfWork unitOfWork) : IHandler<RegisterUserCommand, string>
{
    private readonly IUserWriteOnlyRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<string> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = request.data!.DtoToEntity();

        await _userRepository.CreateUserAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();

        return user.Id.ToString();
    }
}