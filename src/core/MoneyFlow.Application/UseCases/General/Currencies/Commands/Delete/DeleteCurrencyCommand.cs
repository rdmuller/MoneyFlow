using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Delete;

public sealed record DeleteCurrencyCommand(Guid ExternalId) : ICommand;
