using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Infra.DataAccess;

namespace MoneyFlow.Infra.Repositories;

internal sealed class UserRepository : BaseRepository<User>, IUserWriteOnlyRepository, IUserReadRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);

    public async Task<bool> ExistUserWithEmailAsync(Email email, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Email.Equals(email), cancellationToken);

    public async Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

    public async Task<User?> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default)
        => await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

    public async Task<User?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.ExternalId.Equals(externalId), cancellationToken);
}
