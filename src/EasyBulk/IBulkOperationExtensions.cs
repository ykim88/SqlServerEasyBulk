using System;
using System.Linq.Expressions;

namespace EasyBulk
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
    }
}