using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CosmosDb;

public static class ConfigureCosmosDb
{
    public static IServiceCollection ConfigureCosmosDbContainer(
        this IServiceCollection services, 
        IConfiguration configuration,
        string env)
    {
        var endpoint = configuration["CosmosDbServiceEndpoint"];
        var authKey = configuration["CosmosDbServiceAuthKey"];

        if (string.IsNullOrEmpty(endpoint))
            throw new InvalidOperationException("CosmosDbServiceEndpoint is not set in the configuration");

        if (string.IsNullOrEmpty(authKey))
            throw new InvalidOperationException("CosmosDbServiceAuthKey is not set in the configuration");

        var cosmosClient = new CosmosClient(endpoint, authKey);

        if (env == "integration-test")
        {
            var databaseResponse = cosmosClient.CreateDatabaseIfNotExistsAsync(Constants.CosmosDbProperties.TestDatabaseName).GetAwaiter().GetResult();
            var containerResponse = databaseResponse.Database.CreateContainerIfNotExistsAsync(
                new ContainerProperties(Constants.CosmosDbProperties.TestCollectionName, Constants.CosmosDbProperties.PartitionKeyPath)
            ).GetAwaiter().GetResult();

            services.AddSingleton(containerResponse.Container);
            services.AddSingleton(cosmosClient);
            return services;
        }
        else
        {
            var databaseResponse = cosmosClient.CreateDatabaseIfNotExistsAsync(Constants.CosmosDbProperties.DatabaseName).GetAwaiter().GetResult();
            var containerResponse = databaseResponse.Database.CreateContainerIfNotExistsAsync(
                new ContainerProperties(Constants.CosmosDbProperties.CollectionName, Constants.CosmosDbProperties.PartitionKeyPath)
            ).GetAwaiter().GetResult();
         
            services.AddSingleton(containerResponse.Container);
            services.AddSingleton(cosmosClient);
            return services;
        }
    }
}