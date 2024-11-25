using Infrastructure;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Integration.Tests.CosmosDb;

public abstract class CosmosDbTestFixture
{
    protected CosmosClient CosmosDbClient;

    [SetUp]
    public async Task Setup()
    {
        var configuration = GetConfiguration();
        var cosmosConnection = GetCosmosDbConnection(configuration);

        CosmosDbClient = new CosmosClient(cosmosConnection.ConnectionString, new CosmosClientOptions
        {
            ConnectionMode = ConnectionMode.Gateway,
            HttpClientFactory = () => cosmosConnection.client,
        });

        await ConfigureTestDatabase();
        await DoSetup();
    }

    [TearDown]
    public async Task Teardown()
    {
        await UsageTestContainer.DeleteContainerAsync();

        DoTeardown();

        if (CosmosDbClient == null)
            return;

        CosmosDbClient.Dispose();
        CosmosDbClient = null;
    }

    private static IConfiguration GetConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables();

        configuration.AddJsonFile("local.integrationTestsSettings.json", true);


        return configuration.Build();
    }

    private static (HttpClient client, string ConnectionString) GetCosmosDbConnection(IConfiguration configuration)
    {
        var endpoint = configuration[Constants.CosmosDbProperties.CosmosDbServiceEndpoint];
        var authkey = configuration[Constants.CosmosDbProperties.CosmosDbServiceAuthKey];

        if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(authkey))
        {
            throw new InvalidOperationException("Cosmos DB connection details are missing in the configuration");
        }

        var connectionString = $"AccountEndpoint={endpoint};AccountKey={authkey}";
        var client = new HttpClient();

        return (client, connectionString);
    }

    private async Task ConfigureTestDatabase()
    {
        var databaseResponse = await CosmosDbClient.CreateDatabaseIfNotExistsAsync(Constants.CosmosDbProperties.TestDatabaseName, 1000);
        var containerProperties = new ContainerProperties(Constants.CosmosDbProperties.TestCollectionName, Constants.CosmosDbProperties.PartitionKeyPath);
        await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerProperties);
    }

    protected Container UsageTestContainer => CosmosDbClient.GetContainer(Constants.CosmosDbProperties.TestDatabaseName, Constants.CosmosDbProperties.TestCollectionName);

    public virtual Task DoSetup() { return Task.CompletedTask; }
    public virtual Task DoTeardown() { return Task.CompletedTask; }
}
