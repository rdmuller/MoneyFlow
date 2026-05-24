using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Delete;

public sealed record DeleteCurrencyCommand(Guid ExternalId) : ICommand;
