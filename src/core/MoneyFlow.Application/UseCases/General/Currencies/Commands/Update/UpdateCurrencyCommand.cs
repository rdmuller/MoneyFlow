using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Update;

public sealed record UpdateCurrencyCommand(
    Guid? ExternalId,
    string? Name,
    string? Symbol,
    bool? Active) : ICommand;
