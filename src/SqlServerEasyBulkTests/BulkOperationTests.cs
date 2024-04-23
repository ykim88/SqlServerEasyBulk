using System.Data;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using SqlServerEasyBulk;
using SqlServerEasyBulk.Extensions;

namespace SqlServerEasyBulkTests;

public class BulkOperationTests
{
    [Test]
    public async Task EmptyDataList()
    {
        var executorSpy = new BulkExecutorSpy();
        var bulkOperation = new BulkOperation<TestObject>("DoNotMatter", executorSpy);

        await bulkOperation.ExecuteAsync(Enumerable.Empty<TestObject>());

        executorSpy.WasCall.Should().BeFalse();
    }

    [Test]
    public async Task DataListNotEmpty()
    {
        const string DestinationTable = "DoNotMatter";
        var executorSpy = new BulkExecutorSpy();
        var bulkOperation = new BulkOperation<TestObject>(DestinationTable, executorSpy);
        var data = Enumerable.Range(0, 100)
        .Select(i => new TestObject(i, $"{i}", Convert.ToDecimal(i), Convert.ToDouble(i), true))
        .ToList();
        var ctSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        var expectedDataTable = TestUtil.CreateDataTable(DestinationTable, data);

        await bulkOperation
        .AutoMapColumn(x => x.IntColumn)
        .AutoMapColumn(x => x.StringColumn)
        .AutoMapColumn(x => x.DecimalColumn)
        .AutoMapColumn(x => x.FloatColumn)
        .AutoMapColumn(x => x.BitColumn)
        .ExecuteAsync(data, SqlBulkCopyOptions.KeepNulls, ctSource.Token);

        executorSpy.WasCall.Should().BeTrue();
        executorSpy.ExecuteAsyncParameters!.Table.Should().BeEquivalentTo(expectedDataTable);
        executorSpy.ExecuteAsyncParameters!.CancellationToken.Should().NotBeEquivalentTo(CancellationToken.None);
        executorSpy.ExecuteAsyncParameters!.CancellationToken.Should().BeEquivalentTo(ctSource.Token);
        executorSpy.ExecuteAsyncParameters!.Option.Should().Be(SqlBulkCopyOptions.KeepNulls);
    }

    [Test]
    public async Task PassCancellationToken()
    {
        const string DestinationTable = "DoNotMatter";
        var executorSpy = new BulkExecutorSpy();
        var bulkOperation = new BulkOperation<TestObject>(DestinationTable, executorSpy);
        var data = Enumerable.Range(0, 1)
        .Select(i => new TestObject(i, $"{i}", Convert.ToDecimal(i), Convert.ToDouble(i), true))
        .ToList();
        var ctSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        var expectedDataTable = TestUtil.CreateDataTable(DestinationTable, data);

        await bulkOperation
        .AutoMapColumn(x => x.IntColumn)
        .AutoMapColumn(x => x.StringColumn)
        .AutoMapColumn(x => x.DecimalColumn)
        .AutoMapColumn(x => x.FloatColumn)
        .AutoMapColumn(x => x.BitColumn)
        .ExecuteAsync(data, ctSource.Token);

        executorSpy.ExecuteAsyncParameters!.Table.Should().BeEquivalentTo(expectedDataTable);
        executorSpy.ExecuteAsyncParameters!.CancellationToken.Should().NotBeEquivalentTo(CancellationToken.None);
        executorSpy.ExecuteAsyncParameters!.CancellationToken.Should().BeEquivalentTo(ctSource.Token);
        executorSpy.ExecuteAsyncParameters!.Option.Should().Be(SqlBulkCopyOptions.Default);
    }

    [Test]
    public async Task PassSqlBulkOptions()
    {
        const string DestinationTable = "DoNotMatter";
        var executorSpy = new BulkExecutorSpy();
        var bulkOperation = new BulkOperation<TestObject>(DestinationTable, executorSpy);
        var data = Enumerable.Range(0, 1)
        .Select(i => new TestObject(i, $"{i}", Convert.ToDecimal(i), Convert.ToDouble(i), true))
        .ToList();
        var expectedDataTable = TestUtil.CreateDataTable(DestinationTable, data);

        await bulkOperation
        .AutoMapColumn(x => x.IntColumn)
        .AutoMapColumn(x => x.StringColumn)
        .AutoMapColumn(x => x.DecimalColumn)
        .AutoMapColumn(x => x.FloatColumn)
        .AutoMapColumn(x => x.BitColumn)
        .ExecuteAsync(data, SqlBulkCopyOptions.FireTriggers);

        executorSpy.ExecuteAsyncParameters!.Table.Should().BeEquivalentTo(expectedDataTable);
        executorSpy.ExecuteAsyncParameters!.CancellationToken.Should().BeEquivalentTo(CancellationToken.None);
        executorSpy.ExecuteAsyncParameters!.Option.Should().Be(SqlBulkCopyOptions.FireTriggers);
    }
}


internal class BulkExecutorSpy : IBulkCopyExecutor
{
    public Parameters? ExecuteAsyncParameters { get; private set; }
    public bool WasCall => ExecuteAsyncParameters != null;

    public Task ExecuteAsync(DataTable table, SqlBulkCopyOptions option, CancellationToken cancellationToken)
    {
        ExecuteAsyncParameters = new(table, option, cancellationToken);

        return Task.CompletedTask;
    }

    internal record Parameters(DataTable Table, SqlBulkCopyOptions Option, CancellationToken CancellationToken);
}
