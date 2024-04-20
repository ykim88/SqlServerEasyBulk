using System.Linq.Expressions;

namespace EasyBulk;

public interface IBulkOperation<T>
{
    public IBulkOperation<T> MapColumn<TData>(string columnName, Expression<Func<T, TData>> dataSelector);
    public Task ExecuteAsync(IEnumerable<T> data);
}