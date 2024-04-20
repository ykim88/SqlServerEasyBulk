﻿using System.Data;
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
    private IReadOnlyCollection<IColumnDataMapper<T>> _mappings;
    private DataTable _table;

    public BulkOperation(string destinationTable)
    {
        _tableName = destinationTable;
    }

    public Task Execute(SqlConnection connection)
    {
        var executor = new BulkCopyExecutor(connection);
        return executor.Execute(_table);
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
        _mappings = mappings.ToList();
        _table = new DataTable(_tableName);
        foreach (var mapping in _mappings)
            _table.Columns.Add(mapping.ColumnName, mapping.ColumnType);
    }
}
