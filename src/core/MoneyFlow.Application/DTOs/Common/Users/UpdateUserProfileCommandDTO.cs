using MoneyFlow.Domain.Common.Entities.Users;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.Common.Users;

public record UpdateUserProfileCommandDTO
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    public User DtoToEntity() => new User
    {
        Name = Name ?? string.Empty,
        Email = Email ?? string.Empty
    };
}
