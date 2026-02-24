using MoneyFlow.Domain.General.Entities.Currencies;

namespace MoneyFlow.Infra.DataAccess.Repositories;

internal class CurrencyRepository : BaseRepository<Currency>, ICurrencyWriteRepository
{
    public CurrencyRepository(ApplicationDbContext dbContext) : base(dbContext) {}
}
