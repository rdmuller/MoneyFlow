using MoneyFlow.Domain.Entities;
using MoneyFlow.Domain.Repositories.Users;

namespace MoneyFlow.Infra.DataAccess.Repositories;

public class UserRepository(ApplicationDbContext dbcontext) : IUserWriteOnlyRepository
{
    private readonly ApplicationDbContext _dbContext = dbcontext;
    public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }
}