using SharedKernel.Services;

namespace MoneyFlow.Infra.Services;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}