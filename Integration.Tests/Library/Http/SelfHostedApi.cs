using Api.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.TestHelpers;

namespace Integration.Tests.Library.Http;

public sealed class SelfHostedApi : WebApplicationFactory<Startup>
{
    public FakeBus Bus { get; } = new FakeBus();

    private readonly string _connectionString;

    private bool _disposed;

    public SelfHostedApi(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("local.integrationTestsSettings.json", optional: false, reloadOnChange: true);

            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _connectionString,
            });
        });

        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.AddHttpClient();
            services.AddScoped<IBus>(_ => Bus);
        });

        builder.UseEnvironment("integration-test");
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Bus.Dispose();
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }
}