using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Delete;

public sealed record DeleteSectorCommand(Guid ExternalId) : ICommand;
