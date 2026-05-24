using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Create;

public sealed record CreateMarketCommand(string? Name) : ICommand<Guid>;