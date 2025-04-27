using MoneyFlow.Common.Communications;

namespace MoneyFlow.Common.Exceptions;

public class BaseException : Exception
{
    public IEnumerable<BaseError> Errors { get; protected set; } = [];
}