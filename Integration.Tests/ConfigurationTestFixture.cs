using Microsoft.Extensions.Configuration;

namespace Integration.Tests;

[TestFixture]
public abstract class ConfigurationTestFixture
{
    protected IConfiguration Configuration { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
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
}