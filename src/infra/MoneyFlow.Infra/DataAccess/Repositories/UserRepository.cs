using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.Entities;
using MoneyFlow.Domain.Repositories.Users;

namespace MoneyFlow.Infra.DataAccess.Repositories;

public class UserRepository(ApplicationDbContext dbcontext) : IUserWriteOnlyRepository, IUserQueryRepository
{
    private readonly ApplicationDbContext _dbContext = dbcontext;
    public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);
    }

    public async Task<bool> ExistUserWithEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Email.Equals(email), cancellationToken);
    }
}