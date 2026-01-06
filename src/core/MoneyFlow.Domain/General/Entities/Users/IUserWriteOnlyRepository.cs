namespace MoneyFlow.Domain.General.Entities.Users;

public interface IUserWriteOnlyRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken = default);

    void Update(User user, CancellationToken cancellationToken = default);

    Task<User> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default);
}
