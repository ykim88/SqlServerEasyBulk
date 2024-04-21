using System.Linq.Expressions;

namespace EasyBulk;

public interface IBulkOperation<T>
{
    IBulkOperation<T> MapColumn(IColumnMapper<T> columnMap);
    Task ExecuteAsync(IEnumerable<T> data);
}