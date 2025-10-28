using MoneyFlow.Domain.General.Entities.Users;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.Common.Users;

public record UpdateUserProfileCommandDTO
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
