using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace EasyBulk.Extensions
{
    public static class IBulkOperationExtensions
    {
        public static IBulkOperation<T> MapColumn<T, TData>(this IBulkOperation<T> bulkOp, string columnName, Func<T, TData> dataSelector)
        {
            var map = new ColumnMapper<T, TData>(columnName, dataSelector);
            return bulkOp.MapColumn(map);
        }

        public static IBulkOperation<T> AutoMapColumn<T, TData>(this IBulkOperation<T> bulkOp, Expression<Func<T, TData>> dataSelector) where T : class
        {
            var map = new ColumnAutoMapper<T, TData>(dataSelector);
            return bulkOp.MapColumn(map);
        }

        public static Task ExecuteAsync<T>(this IBulkOperation<T> bulk, IEnumerable<T> data) 
            => bulk.ExecuteAsync(data, SqlBulkCopyOptions.Default, CancellationToken.None);
        public static Task ExecuteAsync<T>(this IBulkOperation<T> bulk, IEnumerable<T> data, SqlBulkCopyOptions options) 
            => bulk.ExecuteAsync(data, options, CancellationToken.None);
        public static Task ExecuteAsync<T>(this IBulkOperation<T> bulk, IEnumerable<T> data, CancellationToken cancellationToken) 
            => bulk.ExecuteAsync(data, SqlBulkCopyOptions.Default, cancellationToken);        
    }
}