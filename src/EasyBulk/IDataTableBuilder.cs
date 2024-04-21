using System.Collections.Generic;
using System.Data;

namespace EasyBulk
{
    internal interface IDataTableBuilder<T>
    {
        IDataTableBuilder<T> FillWith(IEnumerable<T> data);
        DataTable Build();
    }
}