using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Rebus.TestHelpers;
using Infrastructure.Repository;
using Respawn;
using Integration.Tests.Library.Http;
using Integration.Tests.Library.TestContainers;
using Infrastructure;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Integration.Tests.Library;

public abstract class BaseEndpointTests
{
    protected HttpClient AppHttpClient => _appClient;
    protected FakeBus AppBusMock => _selfHostedApi.Bus;
    protected IServiceProvider AppServices => _selfHostedApi.Services;

    protected DataOperations Data { get; private set; }
    protected SalesPlatformDbContext AppDbContext => AppServices.GetRequiredService<SalesPlatformDbContext>();

    private ContainerizedSqlServerDatabase _database;
    private SelfHostedApi _selfHostedApi;
    private Respawner _spawner;

    private HttpClient _appClient;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _database = new ContainerizedSqlServerDatabase();
        await _database.Start();

        _selfHostedApi = new SelfHostedApi(_database.ConnectionString);
        _appClient = _selfHostedApi.CreateClient();

        Data = new DataOperations(_selfHostedApi.Services);
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
        _appClient?.Dispose();

        if (_selfHostedApi != null)
        {
            await _selfHostedApi.DisposeAsync();
        }

        if (_database != null)
        {
            await _database.DisposeAsync();
        }
    }

    protected static StringContent CreateJsonRequestContent(string json)
    {
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}