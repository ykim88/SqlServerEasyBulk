using System.Collections.Generic;

namespace EasyBulk
{
    internal interface IDataTableColumnMapping<T>
    {
        IDataTableBuilder<T> ColumnsMapping(IReadOnlyCollection<IColumnMapper<T>> columnMappers);
    }
}
