namespace MoneyFlow.Common.Services;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
