using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Users;

namespace MoneyFlow.Infra.DataAccess.Repositories;

internal sealed class UserRepository : BaseRepository<User>, IUserWriteOnlyRepository, IUserReadRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

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