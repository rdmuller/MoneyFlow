namespace MoneyFlow.Domain.General.Entities.Currencies;

public interface ICurrencyWriteRepository
{
    Task CreateAsync(Currency currency, CancellationToken cancellationToken = default);
}