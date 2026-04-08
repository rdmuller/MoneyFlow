using SharedKernel.Abstractions;

namespace SharedKernel.Exceptions;

public class DataBaseException : BaseException
{
    public DataBaseException(Error error) : base(error)
    {
    }

    public static DataBaseException DuplicatedUniqueKey(string message)
        => new DataBaseException(Error.RecordAlreadyExists(message));

    public static DataBaseException RecordNotFound(string message)
        => new DataBaseException(Error.RecordNotFound(message));
}
