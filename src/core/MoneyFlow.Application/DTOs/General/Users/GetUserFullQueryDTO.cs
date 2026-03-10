using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Enums;

namespace MoneyFlow.Application.DTOs.General.Users;

public record GetUserFullQueryDTO
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Role { get; set; }

    public static GetUserFullQueryDTO EntityToDTO(User user)
    {
        return new GetUserFullQueryDTO
        {
            Email = user.Email.Value,
            Name = user.Name,
            Role = user.Role
        };
    }
}
