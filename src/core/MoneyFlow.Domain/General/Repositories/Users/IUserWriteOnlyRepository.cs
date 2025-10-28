using MoneyFlow.Domain.General.Entities.Users;

namespace MoneyFlow.Domain.General.Repositories.Users;

public interface IUserWriteOnlyRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken = default);

    void UpdateUser(User user, CancellationToken cancellationToken = default);

    Task<User> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default);
}
