using Api.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.TestHelpers;

namespace Integration.Tests.Library.Http;

public sealed class SelfHostedApi : WebApplicationFactory<Program>
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
        builder.ConfigureAppConfiguration((_, conf) => conf.AddInMemoryCollection(CreateConfiguration()));

        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.AddHttpClient();
            services.AddScoped<IBus>(_ => Bus);
        });

        /// Needed for the API service to not start Rebus.
        /// <see cref="Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder, IWebHostEnvironment)"/>.
        builder.UseEnvironment("integration-test");
    }

    private Dictionary<string, string?> CreateConfiguration()
    {
        return new(StringComparer.Ordinal)
        {
            ["SqlDbConnectionString"] = _connectionString,
        };
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