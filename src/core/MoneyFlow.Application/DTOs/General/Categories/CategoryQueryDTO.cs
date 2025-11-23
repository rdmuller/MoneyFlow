using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.General.Categories;
public class CategoryQueryDTO
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; } = true;
}
