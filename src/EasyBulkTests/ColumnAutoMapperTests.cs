using EasyBulk;
using FluentAssertions;

namespace EasyBulkTests;

public class ColumnAutoMapperTests
{
    [Test]
    public void AutoMapPropertyInt()
    {
        var obj = new TestObject(1, "", 1m, 1.0, true);

        var autoMapper = new ColumnAutoMapper<TestObject, int>(t => t.IntColumn);

        autoMapper.ColumnName.Should().Be(nameof(TestObject.IntColumn));
        autoMapper.ColumnType.Should().Be(typeof(int));
        autoMapper.DataSelector(obj).Should().Be(obj.IntColumn);
    }

    [Test]
    public void AutoMapPropertyBool()
    {
        var obj = new TestObject(1, "", 1m, 1.0, true);

        var autoMapper = new ColumnAutoMapper<TestObject, bool>(t => t.BitColumn);

        autoMapper.ColumnName.Should().Be(nameof(TestObject.BitColumn));
        autoMapper.ColumnType.Should().Be(typeof(bool));
        autoMapper.DataSelector(obj).Should().Be(obj.BitColumn);
    }

    [Test]
    public void AutoMapFieldString()
    {
        var obj = new TestObjectFields("Stringvalue", null);

        var autoMapper = new ColumnAutoMapper<TestObjectFields, string>(t => t.StringField);

        autoMapper.ColumnName.Should().Be(nameof(TestObjectFields.StringField));
        autoMapper.ColumnType.Should().Be(typeof(string));
        autoMapper.DataSelector(obj).Should().Be(obj.StringField);
    }

    [Test]
    public void AutoMapFieldNullableDecimal()
    {
        var obj = new TestObjectFields(string.Empty, 1m);

        var autoMapper = new ColumnAutoMapper<TestObjectFields, decimal?>(t => t.NullableDecimalField);

        autoMapper.ColumnName.Should().Be(nameof(TestObjectFields.NullableDecimalField));
        autoMapper.ColumnType.Should().Be(typeof(decimal));
        autoMapper.DataSelector(obj).Should().Be(obj.NullableDecimalField);
    }

    private class TestObjectFields
    {
        public TestObjectFields(string stringField, decimal? nullableDecimal)
        {
            StringField = stringField;
            NullableDecimalField = nullableDecimal;
        }

        public readonly string StringField;
        public readonly decimal? NullableDecimalField;
    }
}
