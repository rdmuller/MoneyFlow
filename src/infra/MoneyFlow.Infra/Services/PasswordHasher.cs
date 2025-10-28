using BC = BCrypt.Net.BCrypt;
using MoneyFlow.Domain.General.Security;

namespace MoneyFlow.Infra.Services;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BC.HashPassword(password);

    public bool Verify(string password, string hashedPassword) => BC.Verify(password, hashedPassword);
}