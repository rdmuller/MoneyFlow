using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Update;

public sealed record UpdateCurrencyCommand(
    Guid? ExternalId,
    string? Name,
    string? Symbol,
    bool? Active) : ICommand;
