using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.TestHelpers;
using Api.Service;

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

        builder.ConfigureTestServices(services =>
        {
            services.AddHttpClient();
            services.AddScoped<IBus>(_ => Bus);
        });

        builder.UseEnvironment("IntegrationTest");
    }

    private Dictionary<string, string?> CreateConfiguration()
    {
        return new(StringComparer.Ordinal)
        {
            ["ConnectionStrings:DefaultConnection"] = _connectionString,
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
