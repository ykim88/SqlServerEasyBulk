using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NUnit.Framework.Legacy;
using EasyBulk;
using EasyBulk.Extensions;

namespace EasyBulkTests.Integrations;

public class BulkTests
{
    [Test]
    public async Task FillOneIntColumn()
    {
        var data = Enumerable.Range(0, 100).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 int)");
        var dataMapping = new[] { new ColumnMapper<int, int>("Column1", i => i) };

        await connection.Bulk<int>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<int>("SELECT Column1 FROM Test");
        CollectionAssert.AreEquivalent(data, result);
    }

    [Test]
    public async Task FillOneIntNullableColumn()
    {
        var data = Enumerable.Range(0, 100).Select<int, int?>(_ => null).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 int NULL)");
        var dataMapping = new[] { new ColumnMapper<int?, int?>("Column1", i => i) };

        await connection.Bulk<int?>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<int?>("SELECT Column1 FROM Test");
        CollectionAssert.AreEquivalent(data, result);
    }

    [Test]
    public async Task FillOneStringColumn()
    {
        var data = Enumerable.Range(0, 100).Select(i => "abcdefghimnopqrstuvz{i}").ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 nvarchar(25))");
        var dataMapping = new[] { new ColumnMapper<string, string>("Column1", i => i) };

        await connection.Bulk<string>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<string>("SELECT Column1 FROM Test");
        CollectionAssert.AreEquivalent(data, result);
    }

    [Test]
    public async Task FillOneStringNullColumn()
    {
        var data = Enumerable.Range(0, 100).Select<int, string>(_ => null).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 nvarchar(25) NULL)");
        var dataMapping = new[] { new ColumnMapper<string, string>("Column1", i => i) };

        await connection.Bulk<string>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<string>("SELECT Column1 FROM Test");
        CollectionAssert.AreEquivalent(data, result);
    }

    [Test]
    public async Task FillOneDecimalColumn()
    {
        var data = Enumerable.Range(0, 100).Select(i => Convert.ToDecimal(i)).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 decimal(10,4))");
        var dataMapping = new[] { new ColumnMapper<decimal, decimal>("Column1", i => i) };

        await connection.Bulk<decimal>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<decimal>("SELECT Column1 FROM Test");
        CollectionAssert.AreEquivalent(data, result);
    }

    [Test]
    public async Task FillOneDecimalNullColumn()
    {
        var data = Enumerable.Range(0, 100).Select<int, decimal?>(_ => null).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 decimal(10,4) NULL)");
        var dataMapping = new[] { new ColumnMapper<decimal?, decimal?>("Column1", i => i) };

        await connection.Bulk<decimal?>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<decimal?>("SELECT Column1 FROM Test");
        CollectionAssert.AreEquivalent(data, result);
    }

    [TearDown]
    public async Task DropTable()
    {
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("DROP TABLE Test");
    }
}