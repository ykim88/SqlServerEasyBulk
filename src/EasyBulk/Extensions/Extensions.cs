using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace EasyBulk.Extensions
{
    public static class Extensions
    {
        public static IBulkOperation<T> Bulk<T>(this SqlConnection connection, string destinationTableName)
        {
            var bulkExecutor = new SqlBulkCopyExecutor(connection, default);
            return new BulkOperation<T>(destinationTableName, bulkExecutor);
        }

        public static IBulkOperation<T> Bulk<T>(this SqlConnection connection, SqlTransaction transaction, string destinationTableName)
        {
            var bulkExecutor = new SqlBulkCopyExecutor(connection, transaction);
            return new BulkOperation<T>(destinationTableName, bulkExecutor);
        }
    }
}