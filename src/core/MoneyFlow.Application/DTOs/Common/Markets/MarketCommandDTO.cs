using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.Common.Markets;

public class MarketCommandDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; }
}
