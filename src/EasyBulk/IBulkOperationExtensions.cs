using System.Linq.Expressions;

namespace EasyBulk;

public static class IBulkOperationExtensions
{
    public static IBulkOperation<T> MapColumn<T, TData>(this IBulkOperation<T> bulkOp, string columnName, Expression<Func<T, TData>> dataSelector)
    {
        var map = new ColumnMapper<T,TData>(columnName, dataSelector);
        return bulkOp.MapColumn(map);
    }
}