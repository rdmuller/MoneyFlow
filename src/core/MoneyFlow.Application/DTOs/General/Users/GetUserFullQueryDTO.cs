using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Enums;
using System.Text.Json.Serialization;

namespace MoneyFlow.Application.DTOs.General.Users;

public record GetUserFullQueryDTO
{
    [JsonIgnore]
    public long Id { get; set; }

    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Role { get; set; }

    public static GetUserFullQueryDTO EntityToDTO(User user)
    {
        return new GetUserFullQueryDTO
        {
            Id = user.Id,
            Email = user.Email.Value,
            Name = user.Name,
            Role = user.Role
        };
    }
}
