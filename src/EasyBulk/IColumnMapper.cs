namespace EasyBulk;

public interface IColumnMapper<T>
{
    public string ColumnName {get;}
    public Type ColumnType {get;}
    public object DataSelector(T obj);
}