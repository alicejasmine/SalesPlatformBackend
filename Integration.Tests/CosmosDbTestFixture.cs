using Infrastructure;
using Microsoft.Azure.Cosmos;


namespace Integration.Tests;

public abstract class CosmosDbTestFixture
{
    protected CosmosClient CosmosDbClient;

    [SetUp]
    public async Task Setup()
    {
        var cosmosConnection = GetCosmosDbConnection();

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
    
    private static (HttpClient client, string ConnectionString) GetCosmosDbConnection()
    {
        var endpoint = Environment.GetEnvironmentVariable("CosmosDbServiceEndpoint");
        var authkey = Environment.GetEnvironmentVariable("CosmosDbServiceAuthKey");
        
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
        var containerProperties = new ContainerProperties(Constants.CosmosDbProperties.CollectionName, Constants.CosmosDbProperties.PartitionKeyPath);
        await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerProperties);
    }
    
    protected Container UsageTestContainer => CosmosDbClient.GetContainer(Constants.CosmosDbProperties.TestDatabaseName, Constants.CosmosDbProperties.CollectionName);

    public virtual Task DoSetup() { return Task.CompletedTask; }
    public virtual Task DoTeardown() { return Task.CompletedTask; }
}
