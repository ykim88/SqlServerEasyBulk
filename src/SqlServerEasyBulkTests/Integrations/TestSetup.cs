using Dapper;
using Microsoft.Data.SqlClient;

namespace SqlServerEasyBulkTests.Integrations;

[SetUpFixture]
public class TestSetup
{
    private static readonly string TestDatabseName = $"TestDB_{DateTime.UtcNow:yyyyMMddHHmmssfff}";

    public static string TestDbConnectionString { get; private set; }

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
        await connection.ExecuteAsync($"CREATE DATABASE {TestDatabseName}");

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
    public async Task DropAsync()
    {
        using var connection = new SqlConnection(TestDbConnectionString);
        await connection.OpenAsync();

        await connection.ChangeDatabaseAsync("master");

        await connection.ExecuteAsync($"DROP DATABASE {TestDatabseName}");
    }
}
