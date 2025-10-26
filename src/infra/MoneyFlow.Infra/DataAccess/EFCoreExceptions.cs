using Microsoft.Data.SqlClient;

namespace MoneyFlow.Infra.DataAccess;

internal static class EFCoreExceptions
{
    public static bool IsUniqueConstraintViolation(this Exception exception)
    {
        if (exception.InnerException is SqlException sqlException)
        {
            // SQL Server error code for unique constraint violation
            return sqlException.Number == 2627 || sqlException.Number == 2601;
        }
        return false;
    }
}