using MoneyFlow.Domain.Entities;

namespace MoneyFlow.Application.DTOs.Users;

public record GetUserFullQueryDTO
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public static GetUserFullQueryDTO EntityToDTO(User user)
    {
        return new GetUserFullQueryDTO
        {
            Email = user.Email,
            Name = user.Name
        };
    }
}
