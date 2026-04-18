using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Update;

public sealed record UpdateUserProfileCommand(string Name, string Email) : ICommand;