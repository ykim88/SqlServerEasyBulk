namespace EasyBulk;

internal interface IDataTableColumnMapping<T>
{
    IDataTableBuilder<T> ColumnsMapping(IReadOnlyCollection<IColumnMapper<T>> columnMappers);
}
