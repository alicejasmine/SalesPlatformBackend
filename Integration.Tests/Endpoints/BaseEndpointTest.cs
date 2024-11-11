using Moq;
using ApplicationServices.Sample;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Umbraco.Cloud.Usage.Environment.Integration.Tests.Endpoints;

public abstract class BaseEndpointTest : IDisposable
{
    protected HttpClient Client { get; private set; }
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<ISampleService>();
                    var mockSampleService = new Mock<ISampleService>();

                    services.AddSingleton(mockSampleService.Object);
                });

                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });
            });

        Client = _factory.CreateClient();
    }

    [TearDown]
    public void Dispose()
    {
        Client?.Dispose();
        _factory?.Dispose();
    }

    protected T GetService<T>() where T : notnull
    {
        return _factory.Services.GetRequiredService<T>();
    }
}