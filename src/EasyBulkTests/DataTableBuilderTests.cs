using System.Data;
using EasyBulk;
using FluentAssertions;

namespace EasyBulkTests;

public class DataTableBuilderTests
{
    [Test]
    public void BuildDataTable()
    {
        const string TableName = "TableName";
        var testObjectList = Enumerable.Range(0, 100).Select(i => new TestObject(i, i.ToString(), Convert.ToDecimal(i), Convert.ToDouble(i), i%2==0))
            .ToList();
        var columnMappings = new IColumnMapper<TestObject>[]
        {
            new ColumnMapper<TestObject, int>("IntColumn", obj => obj.IntColumn),
            new ColumnMapper<TestObject, string>("StringColumn", obj => obj.StringColumn),
            new ColumnMapper<TestObject, decimal>("DecimalColumn", obj => obj.DecimalColumn),
            new ColumnMapper<TestObject, double>("FloatColumn", obj => obj.FloatColumn),
            new ColumnMapper<TestObject, bool?>("BitColumn", obj => obj.BitColumn),
        };
        var expectedDataTable = TestUtil.CreateDataTable(TableName, testObjectList);

        var dataTable = DataTableBuilder
            .Create<TestObject>(TableName)
            .ColumnsMapping(columnMappings)
            .FillWith(testObjectList)
            .Build();

        dataTable.Should().BeEquivalentTo(expectedDataTable);
    }

    [Test]
    public void DuplicatedColumnName()
    {
        var testObjectList = Enumerable.Range(0, 100).Select(i => new TestObject(i, i.ToString(), Convert.ToDecimal(i), Convert.ToDouble(i), i%2==0));
        var columnMappings = new IColumnMapper<TestObject>[]
        {
            new ColumnMapper<TestObject, int>("IntColumn", obj => obj.IntColumn),
            new ColumnMapper<TestObject, string>("IntColumn", obj => obj.StringColumn),
        };

        var building = () => DataTableBuilder
            .Create<TestObject>("TableName")
            .ColumnsMapping(columnMappings)
            .FillWith(testObjectList)
            .Build();

        building.Should().Throw<DuplicateNameException>();
    }

    [Test]
    public void Empty()
    {
        const string TableName = "TableName";
        var testObjectList = Array.Empty<TestObject>();
        var columnMappings = new IColumnMapper<TestObject>[]
        {
            new ColumnMapper<TestObject, int>("IntColumn", obj => obj.IntColumn),
            new ColumnMapper<TestObject, string>("IntColumn", obj => obj.StringColumn),
        };
        var expectedDataTable = TestUtil.CreateDataTable(TableName, testObjectList);

        var building = () => DataTableBuilder
            .Create<TestObject>(TableName)
            .ColumnsMapping(columnMappings)
            .FillWith(testObjectList)
            .Build();

        building.Should().Throw<DuplicateNameException>();
    }
}
