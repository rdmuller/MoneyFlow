using MoneyFlow.Domain.Common.Entities.Users;

namespace MoneyFlow.Domain.Common.Repositories.Users;

public interface IUserWriteOnlyRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken = default);

    void UpdateUser(User user, CancellationToken cancellationToken = default);

    Task<User> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default);
}
