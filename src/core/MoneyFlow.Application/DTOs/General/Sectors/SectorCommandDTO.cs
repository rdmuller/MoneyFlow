using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.General.Sectors;

public record SectorCommandDTO(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("active")] bool Active,
    [property: JsonPropertyName("id")] Guid ExternalId,
    [property: JsonPropertyName("category_id")] Guid CategoryExternalId);
