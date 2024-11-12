using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace Integration.Tests;

[TestFixture]
public abstract class ConfigurationTestFixture
{
    protected IConfiguration Configuration { get; private set; }
    protected MsSqlContainer SqlContainer { get; private set; }

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        SqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithPassword("YourStrongPassword123!")
            .Build();

        await SqlContainer.StartAsync();

        var connectionString = SqlContainer.GetConnectionString();
        Environment.SetEnvironmentVariable("sqlconn", connectionString);

        Configuration = GetConfiguration();

        DoSetup();
    }

    private IConfiguration GetConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddEnvironmentVariables();

        configurationBuilder.AddEnvironmentVariables();

        Configure(configurationBuilder);

        return configurationBuilder.Build();
    }

    protected virtual void Configure(IConfigurationBuilder configure) { }

    public virtual void DoSetup() { }

    [OneTimeTearDown]
    public async Task OneTimeTeardown()
    {
        if (SqlContainer != null)
        {
            await SqlContainer.StopAsync();
            await SqlContainer.DisposeAsync();
        }
    }
}