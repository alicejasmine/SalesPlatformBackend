using Infrastructure;
using Integration.Tests.Library.TestContainers;
using Microsoft.EntityFrameworkCore;

namespace Integration.Tests;

[SetUpFixture]
public sealed class DatabaseTestsFixture
{
    public static SalesPlatformDbContext DbContext { get; private set; }
    public static string ConnectionString { get; private set; }

    private ContainerizedSqlServerDatabase _database;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _database = new ContainerizedSqlServerDatabase();
        await _database.Start();
        ConnectionString = _database.ConnectionString;

        var options = new DbContextOptionsBuilder<SalesPlatformDbContext>()
            .UseSqlServer(_database.ConnectionString)
            .Options;

        DbContext = new SalesPlatformDbContext(options);
        await DbContext.Database.MigrateAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (DbContext != null)
        {
            await DbContext.DisposeAsync();
        }

        if (_database != null)
        {
            await _database.DisposeAsync();
        }
    }
}
