using Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Usage;

public class UsageDocumentRepository  : IUsageDocumentRepository
{
    private readonly Container _container;
    public UsageDocumentRepository(Container container)
    {
        _container = container;
    }
    
    public async Task<UsageEntity> CreateUsageDocument(UsageEntity usageEntity)
    {
        var createdEntity = await _container.CreateItemAsync(
            usageEntity,
            new PartitionKey(usageEntity.PartitionKey.ToString())
        );
        return createdEntity.Resource;
    }
}