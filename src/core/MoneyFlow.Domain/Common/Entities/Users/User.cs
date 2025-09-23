using MoneyFlow.Common.Entities;
using MoneyFlow.Domain.Common.Enums;

namespace MoneyFlow.Domain.Common.Entities.Users;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = Roles.USER;
}