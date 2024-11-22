using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CosmosDb;

public static class ConfigureCosmosDb
{
    public static IServiceCollection ConfigureCosmosDbContainer(
        this IServiceCollection services)
    {
        Environment.SetEnvironmentVariable("CosmosDbServiceEndpoint", "https://localhost:8081");
        Environment.SetEnvironmentVariable("CosmosDbServiceAuthKey", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        var endpoint = Environment.GetEnvironmentVariable("CosmosDbServiceEndpoint");
        var authKey = Environment.GetEnvironmentVariable("CosmosDbServiceAuthKey");
            
        if (string.IsNullOrEmpty(endpoint))
            throw new InvalidOperationException("CosmosDbServiceEndpoint is not set in the configuration");

        if (string.IsNullOrEmpty(authKey))
            throw new InvalidOperationException("CosmosDbServiceAuthKey is not set in the configuration");

        var cosmosClient = new CosmosClient(endpoint, authKey);
            
        var response = cosmosClient.CreateDatabaseIfNotExistsAsync(Constants.CosmosDbProperties.DatabaseName).GetAwaiter().GetResult();
        response.Database.CreateContainerIfNotExistsAsync(
                new ContainerProperties(Constants.CosmosDbProperties.CollectionName, Constants.CosmosDbProperties.PartitionKeyPath))
            .GetAwaiter().GetResult();
            
        services.AddSingleton(cosmosClient);

        return services;
    }
}