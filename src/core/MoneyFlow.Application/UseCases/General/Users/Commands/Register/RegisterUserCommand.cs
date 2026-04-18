using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Register;

public sealed record RegisterUserCommand(string Name, string Email, string Password) : ICommand<Guid>;