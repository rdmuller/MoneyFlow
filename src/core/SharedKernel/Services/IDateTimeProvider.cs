namespace SharedKernel.Services;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
