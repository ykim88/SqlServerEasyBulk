using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace EasyBulk
{
    internal class BulkOperation<T> : IBulkOperation<T>
    {
        private readonly string _tableName;
        private readonly IBulkCopyExecutor _executor;
        private List<IColumnMapper<T>> _columnMappings = new List<IColumnMapper<T>>();

        internal BulkOperation(string destinationTable, IBulkCopyExecutor executor)
        {
            _tableName = destinationTable;
            _executor = executor;
        }

        public IBulkOperation<T> MapColumn(IColumnMapper<T> columnMap)
        {
            _columnMappings.Add(columnMap);
            return this;
        }

        public async Task ExecuteAsync(IEnumerable<T> data, SqlBulkCopyOptions options, CancellationToken cancellationToken)
        {
            using (var table = DataTableBuilder.Create<T>(_tableName)
                .ColumnsMapping(_columnMappings)
                .FillWith(data)
                .Build())
            {
                if(table.Rows.Count == 0)
                    return;

                await _executor.ExecuteAsync(table, options, cancellationToken);
            }
        }
    }
}