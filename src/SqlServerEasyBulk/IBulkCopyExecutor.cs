using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SqlServerEasyBulk
{
    internal interface IBulkCopyExecutor
    {
        Task ExecuteAsync(DataTable table, SqlBulkCopyOptions option, CancellationToken cancellationToken);
    }
}