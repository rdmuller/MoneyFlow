using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.General.Categories;

public class CategoryCommandDTO
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; } = true;
}
