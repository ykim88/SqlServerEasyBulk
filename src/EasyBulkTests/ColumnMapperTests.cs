using EasyBulk;
using FluentAssertions;

namespace EasyBulkTests;

public class ColumnMapperTests
{
    [TestCase(1)]
    [TestCase(1.1)]
    [TestCase("thisIsAString")]
    public void MapType<T>(T _)
    {
        var columnMap = new ColumnMapper<T,T>("DoNotMatter", _=>_);

        columnMap.ColumnType.Should().Be(typeof(T));
    }

    [Test]
    public void MapDecimal()
    {
        var columnMap = new ColumnMapper<decimal,decimal>("DoNotMatter", _=>_);

        columnMap.ColumnType.Should().Be(typeof(decimal));
    }

    [Test]
    public void MapNullableInt()
    {
        var columnMap = new ColumnMapper<int?,int?>("DoNotMatter", i=>i);

        columnMap.ColumnType.Should().Be(typeof(int));
    }

    [Test]
    public void MapNullableDecimal()
    {
        var columnMap = new ColumnMapper<decimal?,decimal?>("DoNotMatter", i=>i);

        columnMap.ColumnType.Should().Be(typeof(decimal));
    }

    [Test]
    public void MapNullableDouble()
    {
        var columnMap = new ColumnMapper<double?, double?>("DoNotMatter", i=>i);

        columnMap.ColumnType.Should().Be(typeof(double));
    }
}