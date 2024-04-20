using Microsoft.Data.SqlClient;

namespace EasyBulk;

public static class Extensions
{
    public static IBulkOperation<T> Bulk<T>(this SqlConnection connection, string destinationTableName) 
    => new BulkOperation<T>(destinationTableName, connection);
}