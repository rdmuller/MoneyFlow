namespace SharedKernel.Exceptions;

public class DataBaseException : BaseException
{
    public DataBaseException(string errorCode, string errorMessage) : base(errorCode, errorMessage)
    {
    }

    public static DataBaseException DuplicatedUniqueKey (string message)
    {
        return new DataBaseException("DuplicatedUniqueKey", message);
    }
}
