using System.Data;
using Microsoft.Data.SqlClient;

namespace EasyBulk;

internal class BulkCopyExecutor
{
    private readonly SqlConnection _connection;

    public BulkCopyExecutor(SqlConnection connection)
    {
        _connection = connection;
    }
    
    public async Task ExecuteAsync(DataTable table)
    {
        using var bulk = new SqlBulkCopy(_connection, SqlBulkCopyOptions.Default, null);

        bulk.DestinationTableName = table.TableName;
        foreach (DataColumn column in table.Columns)
            bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);

        await bulk.WriteToServerAsync(table);
    }
}