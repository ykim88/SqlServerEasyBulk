using System.Data;
using Microsoft.Data.SqlClient;

namespace EasyBulk;

public static class Extensions
{
    public static Task Bulk<T>(this SqlConnection connection, string destinationTableName, IEnumerable<T> data, IEnumerable<IColumnDataMapper<T>> mappings)
    {
        var bulk = new BulkOperation<T>(destinationTableName);

        bulk.CreateDataTable(mappings);
        bulk.FillDataTable(data);
        return bulk.Execute(connection);
    }
}

internal class BulkOperation<T>
{
    private readonly string _tableName;
    private IEnumerable<IColumnDataMapper<T>> _mappings;
    private DataTable _table;

    public BulkOperation(string destinationTable)
    {
        _tableName = destinationTable;
    }

    public Task Execute(SqlConnection connection)
    {
        using var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, null);

        bulk.DestinationTableName = _tableName;
        foreach (DataColumn column in _table.Columns)
            bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);

        return bulk.WriteToServerAsync(_table);
    }

    public void FillDataTable(IEnumerable<T> data)
    {
        var rows = data.Select(d =>
        {
            var row = _table.NewRow();

            foreach (var mapping in _mappings)
                row[mapping.ColumnName] = mapping.DataSelector(d) ?? DBNull.Value;

            return row;
        });

        foreach (var row in rows)
            _table.Rows.Add(row);

        _table.AcceptChanges();
    }

    public void CreateDataTable(IEnumerable<IColumnDataMapper<T>> mappings)
    {
        _mappings = mappings;
        _table = new DataTable(_tableName);
        foreach (var mapping in _mappings)
            _table.Columns.Add(mapping.ColumnName, mapping.ColumnType);
    }
}
