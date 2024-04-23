using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using EasyBulk;
using EasyBulk.Extensions;
using FluentAssertions;

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

        await connection.Bulk<int>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<int>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [Test]
    public async Task FillOneIntNullableColumn()
    {
        var data = Enumerable.Range(0, 100).Select<int, int?>(_ => null).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 int NULL)");

        await connection.Bulk<int?>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<int?>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [Test]
    public async Task FillOneStringColumn()
    {
        var data = Enumerable.Range(0, 100).Select(i => "abcdefghimnopqrstuvz{i}").ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 nvarchar(25))");

        await connection.Bulk<string>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<string>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [Test]
    public async Task FillOneStringNullColumn()
    {
        var data = Enumerable.Range(0, 100).Select<int, string>(_ => null).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 nvarchar(25) NULL)");

        await connection.Bulk<string>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<string>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [Test]
    public async Task FillOneDecimalColumn()
    {
        var data = Enumerable.Range(0, 100).Select(i => Convert.ToDecimal(i)).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 decimal(10,4))");

        await connection.Bulk<decimal>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<decimal>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [Test]
    public async Task FillOneDecimalNullColumn()
    {
        var data = Enumerable.Range(0, 100).Select<int, decimal?>(_ => null).ToList();
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 decimal(10,4) NULL)");

        await connection.Bulk<decimal?>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<decimal?>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [Test]
    public async Task FillOneDateTimeOffestColumn()
    {
         using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 DATETIMEOFFSET(7)NOT NULL)");
        var data = Enumerable.Range(0,100)
        .Select(i => DateTimeOffset.UtcNow.AddSeconds(i))
        .ToList();
        
        await connection.Bulk<DateTimeOffset>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<DateTimeOffset>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [Test]
    public async Task FillOneDateTimeOffestNullColumn()
    {
         using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 DATETIMEOFFSET(7) NULL)");
        var data = Enumerable.Range(0,100)
        .Select<int, DateTimeOffset?>(_ => null)
        .ToList();
        
        await connection.Bulk<DateTimeOffset?>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<DateTimeOffset?>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [Test]
    public async Task FillOneDateTimeColumn()
    {
         using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 DATETIME2 NOT NULL)");
        var data = Enumerable.Range(0,100)
        .Select(i => DateTime.UtcNow.AddSeconds(i))
        .ToList();
        
        await connection.Bulk<DateTime>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<DateTime>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [Test]
    public async Task FillOneDateTimeNullColumn()
    {
         using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE Test (Column1 DATETIME2 NULL)");
        var data = Enumerable.Range(0,100)
        .Select<int, DateTime?>(_ => null)
        .ToList();
        
        await connection.Bulk<DateTime?>("Test")
        .MapColumn("Column1", i => i)
        .ExecuteAsync(data);

        var result = await connection.QueryAsync<DateTime?>("SELECT Column1 FROM Test");
        result.Should().BeEquivalentTo(data);
    }

    [TearDown]
    public async Task DropTable()
    {
        using var connection = new SqlConnection(TestSetup.TestDbConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("DROP TABLE Test");
    }
}