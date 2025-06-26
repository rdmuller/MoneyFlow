using MoneyFlow.Domain.Entities;

namespace MoneyFlow.Domain.Repositories.Users;

public interface IUserWriteOnlyRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken = default);

    void UpdateUser(User user, CancellationToken cancellationToken = default);

    Task<User> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default);
}
