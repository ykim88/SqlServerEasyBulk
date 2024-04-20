using System.Linq.Expressions;

namespace EasyBulkTests;

internal class ColumnDataMapping<T, TData> : IColumnDataMapping<T>
{
    public ColumnDataMapping(string columnName, Expression<Func<T, object>> dataSelector)
    {
        ColumnName = columnName;
        ColumnType = GetDataType();
        DataSelector = dataSelector.Compile();
    }
    public string ColumnName {get;}
    public Type ColumnType {get;}
    public Func<T, object> DataSelector {get;}
    private static Type GetDataType()
    {
        return typeof(TData);
    }
}