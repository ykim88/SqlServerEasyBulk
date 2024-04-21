using EasyBulk;

namespace EasyBulkTests;

public class ColumnMapperTests
{
    [TestCase(1)]
    [TestCase(1.1)]
    [TestCase("thisIsAString")]
    public void MapType<T>(T expectedValue)
    {
        var columnMap = new ColumnMapper<T,T>("DoNotMatter", _=>_);

        Assert.That(columnMap.ColumnType, Is.EqualTo(typeof(T)));
    }

    [Test]
    public void MapDecimal()
    {
        var columnMap = new ColumnMapper<decimal,decimal>("DoNotMatter", _=>_);

        Assert.That(columnMap.ColumnType, Is.EqualTo(typeof(decimal)));
    }

    [Test]
    public void MapNullableInt()
    {
        var columnMap = new ColumnMapper<int?,int?>("DoNotMatter", i=>i);

        Assert.That(columnMap.ColumnType, Is.EqualTo(typeof(int)));
    }

    [Test]
    public void MapNullableDecimal()
    {
        var columnMap = new ColumnMapper<decimal?,decimal?>("DoNotMatter", i=>i);

        Assert.That(columnMap.ColumnType, Is.EqualTo(typeof(decimal)));
    }

    [Test]
    public void MapNullableDouble()
    {
        var columnMap = new ColumnMapper<double?, double?>("DoNotMatter", i=>i);

        Assert.That(columnMap.ColumnType, Is.EqualTo(typeof(double)));
    }
}
