using MoneyFlow.Domain.General.Enums;
using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Users;

public sealed class User : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Role { get; private set; } = Roles.USER;

    private User() {}

    public User(long id, string email, string name, string password, string role, Guid? externalId = null, DateTimeOffset? createdDate = null, DateTimeOffset? updatedDate = null)
        : base(id, externalId, createdDate, updatedDate)
    {
        Email = email;
        Name = name;
        Password = password;
        Role = role;
    }

    public static User Create(string name, string email, string password)
    {
        User user = new User(0, email, name, password, "", Guid.CreateVersion7());

        user.CheckRequiredFields();

        return user;
    }

    public void Update()
    {
        CheckRequiredFields();
    }

    protected override void CheckRequiredFields()
    {
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Name), "User name must be provided");
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Email), "User name must be provided");
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Password), "User name must be provided");
    }
}