using MoneyFlow.Application.DTOs.General.Categories;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.General.Currencies;

public class CurrencyQueryDTO 
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("external_id")]
    public Guid? ExternalId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("created_date")]
    public DateTimeOffset? CreatedDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("updated_date")]
    public DateTimeOffset? UpdatedDate { get; set; }
}
