using MoneyFlow.Domain.Common.Entities.Users;

namespace MoneyFlow.Domain.Common.Repositories.Users;

public interface IUserQueryRepository
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistUserWithEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default);
}
