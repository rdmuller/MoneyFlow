using Npgsql;

namespace MoneyFlow.Infra.DataAccess;

internal static class EFCoreExceptions
{
    public static bool IsUniqueConstraintViolation(this Exception exception)
    {
        // if (exception.InnerException is SqlException sqlException)
        if (exception.InnerException is NpgsqlException sqlException)
        {
            // SQL Server error code for unique constraint violation
            //return sqlException.Number == 2627 || sqlException.Number == 2601;
            // Postgres error code for unique constraint violation
            return sqlException.SqlState == "23505";
        }
        return false;
    }
}