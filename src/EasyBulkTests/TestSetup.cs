using Microsoft.Data.SqlClient;

namespace EasyBulkTests;

[SetUpFixture]
public class TestSetup
{
    private static readonly string TestDatabseName = $"TestDB_{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    // private const string BaseConnectionString = "Server=sqlServer;Database=master;User Id=SA;Password=password123!;MultipleActiveResultSets=true;TrustServerCertificate=True";
    public static string TestDbConnectionString {get; private set;}

    [OneTimeSetUp]
    public async Task CreateDBAsync()
    {
        var sqlHost = Environment.GetEnvironmentVariable("SQL_HOST");
        var password = Environment.GetEnvironmentVariable("SQL_PASSWORD");

        var connectionStringBuilder = new SqlConnectionStringBuilder
        {
            InitialCatalog = "master",
            DataSource = sqlHost,
            Password = password,
            UserID = "SA",
            TrustServerCertificate = true
        };

        using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
        await connection.OpenAsync();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"CREATE DATABASE {TestDatabseName}";
        await cmd.ExecuteNonQueryAsync();
        await connection.CloseAsync();

        TestDbConnectionString = new SqlConnectionStringBuilder
        {
            InitialCatalog = TestDatabseName,
            DataSource = sqlHost,
            Password = password,
            UserID = "SA",
            TrustServerCertificate = true
        }.ConnectionString;
    }

    [OneTimeTearDown]
    public void Drop()
    {

    }
}
