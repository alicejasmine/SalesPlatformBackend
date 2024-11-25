using System.Net;
using Domain;
using Domain.Entities;
using Microsoft.Azure.Cosmos;

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
    
    public async Task<UsageEntity?> GetUsageEntity(DocumentIdentifier documentIdentifier)
    {
        try
        {
            var response = await _container.ReadItemAsync<UsageEntity>(
                documentIdentifier.Value,
                new PartitionKey(documentIdentifier.EnvironmentId.ToString())
            );
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

}