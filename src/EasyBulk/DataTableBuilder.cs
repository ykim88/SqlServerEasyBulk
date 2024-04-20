using System.Data;

namespace EasyBulk;

internal class DataTableBuilder
{
    public static IDataTableColumnMapping<T> Create<T>(string tableName) => new GenericsDataTableBuilder<T>(tableName);

    private class GenericsDataTableBuilder<T> : IDataTableBuilder<T>, IDataTableColumnMapping<T>
    {
        private readonly string _tableName;
        private IReadOnlyCollection<IColumnDataMapper<T>> _columnMappings;
        private IEnumerable<T> _data;

        public GenericsDataTableBuilder(string tableName)
        {
            _tableName = tableName;
        }

        public IDataTableBuilder<T> ColumnsMapping(IReadOnlyCollection<IColumnDataMapper<T>> columnMappers)
        {
            _columnMappings = columnMappers;
            return this;
        }

        public IDataTableBuilder<T> FillWith(IEnumerable<T> data)
        {
            _data = data;
            return this;
        }

        public DataTable Build()
        {
            var table = CreateDataTable();
            FillDataTable(table);
            return table;
        }

        private void FillDataTable(DataTable table)
        {
            var rows = _data.Select(d =>
            {
                var row = table.NewRow();

                foreach (var mapping in _columnMappings)
                    row[mapping.ColumnName] = mapping.DataSelector(d) ?? DBNull.Value;

                return row;
            });

            foreach (var row in rows)
                table.Rows.Add(row);

            table.AcceptChanges();
        }

        private DataTable CreateDataTable()
        {
            var table = new DataTable(_tableName);
            foreach (var mapping in _columnMappings)
                table.Columns.Add(mapping.ColumnName, mapping.ColumnType);

            return table;
        }
    }
}