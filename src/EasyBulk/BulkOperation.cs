using System.Linq.Expressions;
using Microsoft.Data.SqlClient;

namespace EasyBulk;

internal class BulkOperation<T> : IBulkOperation<T>
{
    private readonly string _tableName;
    private readonly SqlConnection _connection;
    private List<IColumnDataMapper<T>> _columnMappings = new();

    public BulkOperation(string destinationTable)
    {
        _tableName = destinationTable;
    }

    public BulkOperation(string destinationTable, SqlConnection connection)
    {
        _tableName = destinationTable;
        _connection = connection;
    }

    public IBulkOperation<T> MapColumn<TData>(string columnName, Expression<Func<T, TData>> dataSelector)
    {
        var map = new ColumnDataMapper<T,TData>(columnName, dataSelector);
        _columnMappings.Add(map);
        return this;
    }

    public Task ExecuteAsync(IEnumerable<T> data)
    {
        using var table = DataTableBuilder.Create<T>(_tableName)
            .ColumnsMapping(_columnMappings)
            .FillWith(data)
            .Build();
        var executor = new BulkCopyExecutor(_connection);
        return executor.ExecuteAsync(table);
    }
}
