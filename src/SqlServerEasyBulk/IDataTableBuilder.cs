using System.Collections.Generic;
using System.Data;

namespace SqlServerEasyBulk
{
    internal interface IDataTableBuilder<T>
    {
        IDataTableBuilder<T> FillWith(IEnumerable<T> data);
        DataTable Build();
    }
}