using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Repositories.Users;

namespace MoneyFlow.Infra.DataAccess.Repositories;

public class UserRepository(ApplicationDbContext dbcontext) : IUserWriteOnlyRepository, IUserReadRepository
{
    private readonly ApplicationDbContext _dbContext = dbcontext;
    public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public void UpdateUser(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(user);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);
    }

    public async Task<bool> ExistUserWithEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Email.Equals(email), cancellationToken);
    }

    async Task<User> IUserReadRepository.GetUserByIdAsync(long userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.AsNoTracking().FirstAsync(u => u.Id.Equals(userId), cancellationToken);
    }

    async Task<User> IUserWriteOnlyRepository.GetUserByIdAsync(long userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstAsync(u => u.Id.Equals(userId), cancellationToken);
    }
}