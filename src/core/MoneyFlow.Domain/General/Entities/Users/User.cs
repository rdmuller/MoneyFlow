using MoneyFlow.Domain.General.Enums;
using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Users;

public sealed class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = Roles.USER;

    protected override void CheckRequiredFields()
    {
        throw new NotImplementedException();
    }
}