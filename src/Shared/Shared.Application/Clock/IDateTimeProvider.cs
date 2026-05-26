namespace Shared.Application.Clock;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
