using MoneyFlow.Application.DTOs.General.Categories;
using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.General.Sectors;

public class SectorQueryDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("external_id")]
    public Guid? ExternalId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("created_date")]
    public DateTimeOffset? CreatedDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("updated_date")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [JsonPropertyName("category")]
    public CategoryQueryDTO Category { get; set; } = null!;
}
