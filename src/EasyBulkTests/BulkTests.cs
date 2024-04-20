using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NUnit.Framework.Legacy;

namespace EasyBulkTests;

public class BulkTests
{
    [Test]
    public async Task FillOneIntColumn()
    {
        var data = Enumerable.Range(0,100).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 int)");
        var dataMapping = new[] { new ColumnDataMapping<int, int>("Column1", i=>i)};
        
        await Bulk(connection, "Test", data, dataMapping);

        var result = await connection.QueryAsync<int>("SELECT Column1 FROM Test");
        CollectionAssert.AreEquivalent(data, result);
    }

    [Test]
    public async Task FillOneStringColumn()
    {
        var data = Enumerable.Range(0,100).Select(i => "abcdefghimnopqrstuvz{i}").ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 nvarchar(25))");
        var dataMapping = new[] { new ColumnDataMapping<string, string>("Column1", i=>i)};
        
        await Bulk(connection, "Test", data, dataMapping);

        var result = await connection.QueryAsync<string>("SELECT Column1 FROM Test");
        CollectionAssert.AreEquivalent(data, result);
    }

    [Test]
    public async Task FillOneDecimalColumn()
    {
        var data = Enumerable.Range(0,100).Select(i => Convert.ToDecimal(i)).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 decimal(10,4))");
        var dataMapping = new[] { new ColumnDataMapping<decimal, decimal>("Column1", i=>i)};
        
        await Bulk(connection, "Test", data, dataMapping);

        var result = await connection.QueryAsync<decimal>("SELECT Column1 FROM Test");
        CollectionAssert.AreEquivalent(data, result);
    }

    [TearDown]
    public async Task DropTable()
    {
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("DROP TABLE Test");
    }






    public async Task Bulk<T>(SqlConnection connection, string destinationTableName, IEnumerable<T> data, IEnumerable<IColumnDataMapping<T>> mappings)
    {
        using var table = new DataTable(destinationTableName);
        foreach(var mapping in mappings)
            table.Columns.Add(mapping.ColumnName, mapping.ColumnType);

        var rows = data.Select(d => 
        {
            var row = table.NewRow();
            
            foreach(var mapping in mappings)
                row[mapping.ColumnName] = mapping.DataSelector(d);

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