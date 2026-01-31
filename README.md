# SqlServerEasyBulk

A modern and easy-to-use C# library for performing bulk insert operations in SQL Server with a fluent and intuitive API.

## 🎯 Features

- **Fluent API**: Intuitive and easy-to-use interface
- **Extension Methods**: Seamless integration with `SqlConnection`
- **Auto-mapping**: Automatic column mapping using lambda expressions
- **Manual Mapping**: Fine-grained control with explicit column mapping
- **Async/Await**: Full support for asynchronous operations
- **Transactions**: Built-in support for SQL Server transactions
- **Flexible Options**: Configuration via `SqlBulkCopyOptions`
- **Cancellation Token**: Support for operation cancellation

## 📦 Installation

```bash
dotnet add package SqlServerEasyBulk
```

## 🚀 Usage Examples

### Basic Example - Simple Bulk Insert with Auto-mapping

```csharp
using Microsoft.Data.SqlClient;
using SqlServerEasyBulk.Extensions;

var connectionString = "Server=.;Database=MyDB;";
using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

var data = new List<User>
{
    new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
    new User { Id = 2, Name = "Bob", Email = "bob@example.com" },
    new User { Id = 3, Name = "Charlie", Email = "charlie@example.com" }
};

await connection
    .Bulk<User>("Users")
    .AutoMapColumn(x => x.Id)
    .AutoMapColumn(x => x.Name)
    .AutoMapColumn(x => x.Email)
    .ExecuteAsync(data);
```

### Manual Column Mapping

```csharp
using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

var data = new List<User>
{
    new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
    new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
};

// Map specific columns with custom names if needed
await connection
    .Bulk<User>("Users")
    .MapColumn("UserId", x => x.Id)
    .MapColumn("UserName", x => x.Name)
    .MapColumn("UserEmail", x => x.Email))
    .ExecuteAsync(data);
```

### Bulk Insert with Transaction

```csharp
using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

using var transaction = connection.BeginTransaction();

try
{
    var users = GetUsersFromSource();
    
    await connection
        .Bulk<User>(transaction, "Users")
        .AutoMapColumn(x => x.Id)
        .AutoMapColumn(x => x.Name)
        .AutoMapColumn(x => x.Email)
        .ExecuteAsync(users);
    
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

### Bulk Insert with Options and Cancellation Token

```csharp
using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));

var data = GetLargeDataset();

await connection
    .Bulk<Product>("Products")
    .AutoMapColumn(x => x.ProductId)
    .AutoMapColumn(x => x.Name)
    .AutoMapColumn(x => x.Price)
    .AutoMapColumn(x => x.Stock)
    .ExecuteAsync(data, SqlBulkCopyOptions.KeepNulls, cts.Token);
```

### Manual Mapping with Transaction and Options

```csharp
using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

using var transaction = connection.BeginTransaction();
using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));

try
{
    var products = GetProductsFromSource();
    
    await connection
        .Bulk<Product>(transaction, "Products")
        .MapColumn("ProductId", x => x.Id)
        .MapColumn("ProductName", x => x.Name)
        .MapColumn("Price", x => x.Price)
        .ExecuteAsync(products, SqlBulkCopyOptions.KeepNulls, cts.Token);
    
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

## 🧪 Testing

The library includes comprehensive unit tests to ensure reliability. Tests cover:

- Automatic column mapping
- Manual column mapping
- Operations with and without transactions
- Cancellation token handling
- Custom bulk copy options

## 📝 Important Notes

- If the data collection is empty, the operation is not executed
- Columns must exist in the destination table
- Ensure the `SqlConnection` is open before executing the operation
- If you use a transaction, remember to commit or rollback after the operation
- For auto-mapping, property names must match column names exactly

## 📄 License

MIT License\


**THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.**

## ⚠️ Disclaimer

This library is provided as-is. The author assumes no responsibility for any
data loss, corruption, or other damages that may result from the use of this
library. Users are responsible for:

- Testing thoroughly in their own environment before production use
- Implementing appropriate backup and recovery procedures
- Monitoring and validating data integrity during bulk operations
- Following their organization's data governance policies

Use at your own risk. Always maintain backups of your data.
