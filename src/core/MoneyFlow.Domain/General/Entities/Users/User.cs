using MoneyFlow.Domain.General.Entities.Users.Events;
using MoneyFlow.Domain.General.Enums;
using MoneyFlow.Domain.General.Security;
using SharedKernel.Entities;

namespace MoneyFlow.Domain.General.Entities.Users;

public sealed class User : BaseEntity
{
    public Email Email { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Role { get; private set; } = Roles.USER;

    private User() {}

    public User(long id, Email email, string name, string? password, string? role, Guid? externalId = null, DateTimeOffset? createdDate = null, DateTimeOffset? updatedDate = null)
        : base(id, externalId, createdDate, updatedDate)
    {
        Email = email;
        Name = name;
        Password = password ?? string.Empty;
        Role = role ?? string.Empty;
    }

    public static User Create(string name, Email email, string? password = null, IPasswordHasher? passwordHasher = null)
    {
        User user = new User(0, email, name, "", "", Guid.CreateVersion7());

        user.CheckRequiredFields();

        if (password is not null && passwordHasher is not null)
            user.SetPassword(password, passwordHasher);

        return user;
    }

    public void Update(string name, Email email)
    {
        Name = name;
        Email = email;

        CheckRequiredFields();
    }

    public void ChangePassword(string newPassword, IPasswordHasher passwordHasher)
    {
        SetPassword(newPassword, passwordHasher);

        RaiseDomainEvent(new UserChangePasswordDomainEvent(this));
    }

    private void SetPassword(string password, IPasswordHasher passwordHasher) 
        => Password = passwordHasher.Hash(password);

    protected override void CheckRequiredFields()
    {
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Name), "User name must be provided");
        CheckRequiredField(string.IsNullOrWhiteSpace(this.Email.Value), "Email must be provided");
    }
}