using System.Data;
using Microsoft.Data.SqlClient;

namespace EasyBulk;

public static class Extensions
{
    public static async Task Bulk<T>(this SqlConnection connection, string destinationTableName, IEnumerable<T> data, IEnumerable<IColumnDataMapper<T>> mappings)
    {
        using var table = new DataTable(destinationTableName);
        foreach(var mapping in mappings)
            table.Columns.Add(mapping.ColumnName, mapping.ColumnType);

        var rows = data.Select(d => 
        {
            var row = table.NewRow();
            
            foreach(var mapping in mappings)
                row[mapping.ColumnName] = mapping.DataSelector(d) ?? DBNull.Value;

            return row;
        });

        foreach(var row in rows)
            table.Rows.Add(row);
        
        table.AcceptChanges();

        using var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, null);

        bulk.DestinationTableName = destinationTableName;
        foreach(DataColumn column in table.Columns)
            bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
        
        await bulk.WriteToServerAsync(table);
    }
}
