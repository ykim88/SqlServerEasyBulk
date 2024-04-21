using System.Linq.Expressions;
using Microsoft.Data.SqlClient;

namespace EasyBulk;

internal class BulkOperation<T> : IBulkOperation<T>
{
    private readonly string _tableName;
    private readonly SqlConnection _connection;
    private List<IColumnMapper<T>> _columnMappings = new();

    public BulkOperation(string destinationTable)
    {
        _tableName = destinationTable;
    }

    public BulkOperation(string destinationTable, SqlConnection connection)
    {
        _tableName = destinationTable;
        _connection = connection;
    }

    public IBulkOperation<T> MapColumn(IColumnMapper<T> columnMap)
    {
        _columnMappings.Add(columnMap);
        return this;
    }

    public async Task ExecuteAsync(IEnumerable<T> data)
    {
        using var table = DataTableBuilder.Create<T>(_tableName)
            .ColumnsMapping(_columnMappings)
            .FillWith(data)
            .Build();
        var executor = new BulkCopyExecutor(_connection);
        await executor.ExecuteAsync(table);
    }
}
