using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;

public sealed record UpdateSectorCommand(
    Guid? ExternalId,
    string? Name,
    Guid? CategoryId,
    bool? Active = true) : ICommand;
