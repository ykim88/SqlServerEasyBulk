using System.Data;
using Microsoft.Data.SqlClient;

namespace EasyBulk;

internal class BulkCopyExecutor
{
    private readonly SqlConnection _connection;
    private readonly SqlTransaction? _transaction;

    public BulkCopyExecutor(SqlConnection connection, SqlTransaction? transaction) 
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task ExecuteAsync(DataTable table, SqlBulkCopyOptions option, CancellationToken cancellationToken)
    {
        using var bulk = new SqlBulkCopy(_connection, option, _transaction);

        bulk.DestinationTableName = table.TableName;
        foreach (DataColumn column in table.Columns)
            bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);

        await bulk.WriteToServerAsync(table, cancellationToken);
    }
}