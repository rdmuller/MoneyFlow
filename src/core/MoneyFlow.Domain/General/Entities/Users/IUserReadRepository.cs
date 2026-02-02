namespace MoneyFlow.Domain.General.Entities.Users;

public interface IUserReadRepository
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<bool> ExistUserWithEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User> GetByIdAsync(long userId, CancellationToken cancellationToken = default);
}
