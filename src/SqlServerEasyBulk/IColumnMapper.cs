using System;

namespace SqlServerEasyBulk
{
    public interface IColumnMapper<T>
    {
        string ColumnName { get; }
        Type ColumnType { get; }
        object DataSelector(T obj);
    }
}