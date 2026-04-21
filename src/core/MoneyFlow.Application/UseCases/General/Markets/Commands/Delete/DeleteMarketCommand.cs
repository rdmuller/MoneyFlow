using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;

public sealed record DeleteMarketCommand(Guid ExternalId) : ICommand;