using System.Text.Json.Serialization;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;

public sealed record CreateSectorCommand(
    string Name, 
    [property: JsonPropertyName("category_id")] Guid CategoryExternalId) : ICommand<Guid>;
