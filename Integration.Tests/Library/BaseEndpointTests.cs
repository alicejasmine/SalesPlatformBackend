using Infrastructure;
using Integration.Tests.Library.Http;
using Microsoft.Extensions.DependencyInjection;
using Rebus.TestHelpers;
using System.Text;

namespace Integration.Tests.Library;

public abstract class BaseEndpointTests
{
    protected HttpClient AppHttpClient => _appClient;
    protected FakeBus AppBusMock => _selfHostedApi.Bus;
    protected IServiceProvider AppServices => _selfHostedApi.Services;

    protected DataOperations Data { get; private set; }
    protected SalesPlatformDbContext AppDbContext => AppServices.GetRequiredService<SalesPlatformDbContext>();

    private SelfHostedApi _selfHostedApi;

    private HttpClient _appClient;

    [SetUp]
    public async Task Setup()
    {
        _selfHostedApi = new SelfHostedApi(DatabaseTestsFixture.ConnectionString);
        _appClient = _selfHostedApi.CreateClient();

        Data = new DataOperations(_selfHostedApi.Services);
    }

    [TearDown]
    public async Task TearDown()
    {
        _appClient?.Dispose();

        if (_selfHostedApi != null)
        {
            await _selfHostedApi.DisposeAsync();
        }
    }

    protected static StringContent CreateJsonRequestContent(string json)
    {
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}