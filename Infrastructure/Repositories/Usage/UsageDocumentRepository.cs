using Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Usage;

public class UsageDocumentRepository  : IUsageDocumentRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger<UsageDocumentRepository> _logger;
    private Container Container =>
        _cosmosClient.GetContainer(Constants.CosmosDbProperties.DatabaseName, Constants.CosmosDbProperties.CollectionName);
    public UsageDocumentRepository(CosmosClient cosmosClient, ILogger<UsageDocumentRepository> logger)
    {
        _cosmosClient = cosmosClient;
        _logger = logger;
    }
    
    public async Task<UsageEntity> CreateUsageDocument(UsageEntity usageEntity)
    {
        var createdEntity = await Container.CreateItemAsync(
            usageEntity,
            new PartitionKey(usageEntity.PartitionKey.ToString())
        );
        return createdEntity.Resource;
    }
}