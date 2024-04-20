namespace EasyBulk;

public interface IColumnDataMapper<T>
{
    public string ColumnName {get;}
    public Type ColumnType {get;}
    public Func<T, object> DataSelector {get;}
}