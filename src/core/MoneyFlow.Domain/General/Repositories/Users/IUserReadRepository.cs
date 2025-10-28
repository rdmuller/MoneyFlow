using MoneyFlow.Domain.General.Entities.Users;

namespace MoneyFlow.Domain.General.Repositories.Users;

public interface IUserReadRepository
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistUserWithEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default);
}
