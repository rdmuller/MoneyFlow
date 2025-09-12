using MoneyFlow.Domain.Entities.Users;
using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.Users;

public record RegisterUserCommandDTO
{
    [JsonPropertyName("name")]
    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;


    public User DtoToEntity()
    {
        return new User
        {
            Name = Name,
            Password = Password,
            Email = Email
        };
    }
}