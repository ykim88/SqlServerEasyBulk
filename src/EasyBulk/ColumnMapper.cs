using System.Linq.Expressions;

namespace EasyBulk;

internal class ColumnMapper<T, TData> : IColumnMapper<T>
{
    public readonly Func<T, TData> _dataSelector;

    public ColumnMapper(string columnName, Expression<Func<T, TData>> dataSelector)
    {
        ColumnName = columnName;
        ColumnType = GetDataType();
        _dataSelector = dataSelector.Compile();
    }
    public string ColumnName {get;}
    public Type ColumnType {get;}

    public object DataSelector(T obj) => _dataSelector(obj);

    private static Type GetDataType()
    {
        var dataType = typeof(TData);
        
        var nullableType = Nullable.GetUnderlyingType(dataType);
        
        return nullableType ?? dataType;
    }
}