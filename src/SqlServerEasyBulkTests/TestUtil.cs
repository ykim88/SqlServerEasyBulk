using System.Data;

namespace SqlServerEasyBulkTests;

public class TestUtil
{
    internal static DataTable CreateDataTable(string tableName, IEnumerable<TestObject> testObjectList)
    {
        var table = new DataTable(tableName);
        table.Columns.Add("IntColumn", typeof(int));
        table.Columns.Add("StringColumn", typeof(string));
        table.Columns.Add("DecimalColumn", typeof(decimal));
        table.Columns.Add("FloatColumn", typeof(double));
        table.Columns.Add("BitColumn", typeof(bool));

        var rows = testObjectList
        .Select(obj =>
        {
            var row = table.NewRow();
            row["IntColumn"] = obj.IntColumn;
            row["StringColumn"] = obj.StringColumn;
            row["DecimalColumn"] = obj.DecimalColumn;
            row["FloatColumn"] = obj.FloatColumn;
            row["BitColumn"] = obj.BitColumn;
            return row;
        });

        foreach (var row in rows)
            table.Rows.Add(row);

        table.AcceptChanges();

        return table;
    }
}
