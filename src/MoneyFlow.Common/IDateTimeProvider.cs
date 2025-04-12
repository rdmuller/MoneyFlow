namespace MoneyFlow.Common;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
