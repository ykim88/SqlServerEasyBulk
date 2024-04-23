using System;
using System.Linq.Expressions;

namespace SqlServerEasyBulk
{
    internal class ColumnAutoMapper<T, TData> : IColumnMapper<T> where T : class
    {
        private readonly Func<T, TData> _dataSelector;

        public ColumnAutoMapper(Expression<Func<T, TData>> dataSelector)
        {
            SetTypeAndColumnName(dataSelector);
            _dataSelector = dataSelector.Compile();
        }

        private void SetTypeAndColumnName(Expression<Func<T, TData>> dataSelector)
        {
            if (!(dataSelector.Body is MemberExpression member))
                throw new ArgumentException($"The selected object member is not a property: {dataSelector}");

            ColumnName = member.Member.Name;
            ColumnType = Nullable.GetUnderlyingType(member.Type) ?? member.Type;
        }

        public string ColumnName { get; private set; }

        public Type ColumnType { get; private set; }

        public object DataSelector(T obj)
        {
            return _dataSelector(obj);
        }
    }
}
