using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBulk
{
    public interface IBulkOperation<T>
    {
        IBulkOperation<T> MapColumn(IColumnMapper<T> columnMap);
        Task ExecuteAsync(IEnumerable<T> data);
    }
}