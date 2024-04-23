using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SqlServerEasyBulk
{
    public interface IBulkOperation<T>
    {
        IBulkOperation<T> MapColumn(IColumnMapper<T> columnMap);
        Task ExecuteAsync(IEnumerable<T> data, SqlBulkCopyOptions options, CancellationToken cancellationToken);
    }
}