using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.Common.Users;

public class UpdateUserProfileCommandDTO
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
