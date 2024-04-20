namespace EasyBulk;

internal interface IDataTableColumnMapping<T>
{
    IDataTableBuilder<T> ColumnsMapping(IReadOnlyCollection<IColumnDataMapper<T>> columnMappers);
}
