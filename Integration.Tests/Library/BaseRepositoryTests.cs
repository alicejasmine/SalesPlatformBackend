using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Infrastructure.Repository;
using Integration.Tests.Library.TestContainers;
using Respawn;
using Infrastructure;

namespace Integration.Tests.Library;

[SetUpFixture]
public abstract class BaseRepositoryTests<TRepository> where TRepository : class
{
    protected SalesPlatformDbContext DbContext { get; private set; }

    protected TRepository Repository => CreateRepository();

    private ContainerizedSqlServerDatabase _database;
    private Respawner _spawner;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _database = new ContainerizedSqlServerDatabase();
        await _database.Start();

        var options = new DbContextOptionsBuilder<SalesPlatformDbContext>()
            .UseSqlServer(_database.ConnectionString)
            .Options;

        DbContext = new SalesPlatformDbContext(options);
        await DbContext.Database.MigrateAsync();
    }

    [SetUp]
    public async Task Setup()
    {
        _spawner = await Respawner.CreateAsync(_database.ConnectionString);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _spawner.ResetAsync(_database.ConnectionString);
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

    protected abstract TRepository CreateRepository();
}