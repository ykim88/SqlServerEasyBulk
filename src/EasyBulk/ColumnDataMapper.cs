using System.Linq.Expressions;

namespace EasyBulkTests;

internal class ColumnDataMapper<T, TData> : IColumnDataMapper<T>
{
    public ColumnDataMapper(string columnName, Expression<Func<T, object>> dataSelector)
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
        var dataType = typeof(TData);
        
        var nullableType = Nullable.GetUnderlyingType(dataType);
        
        return nullableType ?? dataType;
    }
}