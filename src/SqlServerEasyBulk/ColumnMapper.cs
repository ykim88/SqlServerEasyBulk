using System;

namespace SqlServerEasyBulk
{
    internal class ColumnMapper<T, TData> : IColumnMapper<T>
    {
        private readonly Func<T, TData> _dataSelector;

        public ColumnMapper(string columnName, Func<T, TData> dataSelector)
        {
            ColumnName = columnName;
            ColumnType = GetDataType();
            _dataSelector = dataSelector;
        }
        public string ColumnName { get; }
        public Type ColumnType { get; }

        public object DataSelector(T obj) => _dataSelector(obj);

        private static Type GetDataType()
        {
            var dataType = typeof(TData);

            var nullableType = Nullable.GetUnderlyingType(dataType);

            return nullableType ?? dataType;
        }
    }
}