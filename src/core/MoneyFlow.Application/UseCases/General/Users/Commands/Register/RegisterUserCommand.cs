using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Register;

public sealed record RegisterUserCommand(string Name, string Email, string Password) : IRequest<BaseResponse<string>>;