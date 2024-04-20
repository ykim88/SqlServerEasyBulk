namespace EasyBulkTests;

public interface IColumnDataMapping<T>
{
    public string ColumnName {get;}
    public Type ColumnType {get;}
    public Func<T, object> DataSelector {get;}
}