namespace EasyBulk;

public interface IColumnDataMapper<T>
{
    public string ColumnName {get;}
    public Type ColumnType {get;}
    public object DataSelector(T obj);
}