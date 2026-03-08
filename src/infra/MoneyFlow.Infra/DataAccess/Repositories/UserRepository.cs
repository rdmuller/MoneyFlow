using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Users;

namespace MoneyFlow.Infra.DataAccess.Repositories;

internal sealed class UserRepository : BaseRepository<User>, IUserWriteOnlyRepository, IUserReadRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);

    public async Task<bool> ExistUserWithEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Email.Equals(email), cancellationToken);

    async Task<User> IUserReadRepository.GetByIdAsync(long userId, CancellationToken cancellationToken)
        => await _dbContext.Users.AsNoTracking().FirstAsync(u => u.Id.Equals(userId), cancellationToken);

    async Task<User> IUserWriteOnlyRepository.GetUserByIdAsync(long userId, CancellationToken cancellationToken)
        => await _dbContext.Users.FirstAsync(u => u.Id.Equals(userId), cancellationToken);

    public async Task<User> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AsNoTracking().FirstAsync(u => u.ExternalId.Equals(externalId), cancellationToken);
}