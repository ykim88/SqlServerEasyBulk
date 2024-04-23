using System.Collections.Generic;

namespace SqlServerEasyBulk
{
    internal interface IDataTableColumnMapping<T>
    {
        IDataTableBuilder<T> ColumnsMapping(IReadOnlyCollection<IColumnMapper<T>> columnMappers);
    }
}
