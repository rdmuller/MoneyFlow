using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;

public sealed record CreateCurrencyCommand(string Name, string Symbol) : ICommand<Guid>;
