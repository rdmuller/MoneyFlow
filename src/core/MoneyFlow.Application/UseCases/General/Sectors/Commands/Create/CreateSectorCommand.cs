using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;

public sealed record CreateSectorCommand(string Name, Guid CategoryExternalId) : ICommand<Guid>;