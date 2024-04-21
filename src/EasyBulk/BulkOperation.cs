using Microsoft.Data.SqlClient;

namespace EasyBulk;

internal class BulkOperation<T> : IBulkOperation<T>
{
    private readonly string _tableName;
    private readonly SqlConnection _connection;
    private readonly SqlTransaction _transaction;
    private List<IColumnMapper<T>> _columnMappings = new();

    internal BulkOperation(string destinationTable, SqlConnection connection) : this(destinationTable, connection, null)
    {}

    internal BulkOperation(string destinationTable, SqlConnection connection, SqlTransaction transaction)
    {
        _tableName = destinationTable;
        _connection = connection;
        _transaction = transaction;
    }

    public IBulkOperation<T> MapColumn(IColumnMapper<T> columnMap)
    {
        _columnMappings.Add(columnMap);
        return this;
    }

    public Task ExecuteAsync(IEnumerable<T> data) => ExecuteAsync(data, SqlBulkCopyOptions.Default, CancellationToken.None);
    public Task ExecuteAsync(IEnumerable<T> data, CancellationToken cancellationToken) => ExecuteAsync(data, SqlBulkCopyOptions.Default, cancellationToken);
    public Task ExecuteAsync(IEnumerable<T> data, SqlBulkCopyOptions options) => ExecuteAsync(data, options, CancellationToken.None);

    public async Task ExecuteAsync(IEnumerable<T> data, SqlBulkCopyOptions options, CancellationToken cancellationToken)
    {
        using var table = DataTableBuilder.Create<T>(_tableName)
            .ColumnsMapping(_columnMappings)
            .FillWith(data)
            .Build();
        var executor = new BulkCopyExecutor(_connection, _transaction);
        await executor.ExecuteAsync(table, options, cancellationToken);
    }
}        
