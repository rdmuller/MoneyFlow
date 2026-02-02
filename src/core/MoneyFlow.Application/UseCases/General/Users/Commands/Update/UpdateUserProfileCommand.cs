using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Users;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Update;

public sealed record UpdateUserProfileCommand(string Name, string Email) : IRequest<BaseResponse<string>>;