using System.Data;
using Dapper;
using EasyBulk;
using FluentAssertions;
using Microsoft.Data.SqlClient;

namespace EasyBulkTests.Integrations;

public class BulkCopyExecutorTests
{
    [Test]
    public async Task Execute()
    {
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (IntColumn int, StringColumn varchar(10), DecimalColumn decimal(10,4), FloatColumn float, BitColumn bit)");
        var executor = new SqlBulkCopyExecutor(connection, null);
        using var table = CreateDatatTable("Test");

        await executor.ExecuteAsync(table, SqlBulkCopyOptions.Default, CancellationToken.None);

        var result = await connection.QueryAsync<TestObject>("SELECT IntColumn, StringColumn, DecimalColumn, FloatColumn, BitColumn FROM Test");
        ReadTable(table).Should().BeEquivalentTo(result);
    }

    [Test]
    public async Task ExecuteWithTransaction()
    {
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (IntColumn int, StringColumn varchar(10), DecimalColumn decimal(10,4), FloatColumn float, BitColumn bit)");
        using SqlTransaction transaction = connection.BeginTransaction();
        var executor = new SqlBulkCopyExecutor(connection, transaction);
        using var table = CreateDatatTable("Test");

        await executor.ExecuteAsync(table, SqlBulkCopyOptions.Default, CancellationToken.None);

        await transaction.CommitAsync();
        var result = await connection.QueryAsync<TestObject>("SELECT IntColumn, StringColumn, DecimalColumn, FloatColumn, BitColumn FROM Test");
        ReadTable(table).Should().BeEquivalentTo(result);
    }

    [Test]
    public async Task RollbackedTransaction()
    {
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (IntColumn int, StringColumn varchar(10), DecimalColumn decimal(10,4), FloatColumn float, BitColumn bit)");
        using SqlTransaction transaction = connection.BeginTransaction();
        var executor = new SqlBulkCopyExecutor(connection, transaction);
        using var table = CreateDatatTable("Test");
        await transaction.RollbackAsync();

        var execute = async () => await executor.ExecuteAsync(table, SqlBulkCopyOptions.Default, CancellationToken.None);

        await execute.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task ClosedConnection()
    {
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.ExecuteAsync("CREATE TABLE Test (IntColumn int, StringColumn varchar(10), DecimalColumn decimal(10,4), FloatColumn float, BitColumn bit)");
        var executor = new SqlBulkCopyExecutor(connection, null);
        using var table = CreateDatatTable("Test");

        var execute = async () => await executor.ExecuteAsync(table, SqlBulkCopyOptions.Default, CancellationToken.None);

        await execute.Should().ThrowExactlyAsync<InvalidOperationException>();
    }

    [TearDown]
    public async Task DropTable()
    {
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("DROP TABLE Test");
    }

    private static IEnumerable<TestObject> ReadTable(DataTable table)
    {
        foreach (DataRow row in table.Rows)
        {
            yield return new TestObject(
                Convert.ToInt32(row["IntColumn"].ToString()),
                row["StringColumn"].ToString(),
                Convert.ToDecimal(row["DecimalColumn"].ToString()),
                Convert.ToDouble(row["FloatColumn"].ToString()),
                Convert.ToBoolean(row["BitColumn"].ToString())

            );
        }
    }

    private static DataTable CreateDatatTable(string tableName)
    {
        var table = new DataTable(tableName);
        table.Columns.Add("IntColumn", typeof(int));
        table.Columns.Add("StringColumn", typeof(string));
        table.Columns.Add("DecimalColumn", typeof(decimal));
        table.Columns.Add("FloatColumn", typeof(double));
        table.Columns.Add("BitColumn", typeof(bool));

        var rows = Enumerable.Range(0, 1000)
        .Select(i =>
        {
            var row = table.NewRow();
            row["IntColumn"] = i;
            row["StringColumn"] = i.ToString();
            row["DecimalColumn"] = Convert.ToDecimal(i);
            row["FloatColumn"] = Convert.ToDouble(i);
            row["BitColumn"] = i % 2 == 0;
            return row;
        });

        foreach (var row in rows)
            table.Rows.Add(row);

        table.AcceptChanges();

        return table;
    }
}
