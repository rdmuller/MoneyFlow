using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.General.Currencies;

public class CurrencyCommandDTO
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; } = true;

}
