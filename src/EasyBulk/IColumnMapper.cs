using System;

namespace EasyBulk
{
    public interface IColumnMapper<T>
    {
        string ColumnName { get; }
        Type ColumnType { get; }
        object DataSelector(T obj);
    }
}