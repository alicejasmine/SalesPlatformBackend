using System.Net;
using Domain.ValueObject;
using Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Repositories.Usage;

public class UsageDocumentRepository : IUsageDocumentRepository
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

    #region seed data
    public async Task SeedUsageDocument(Guid projectId, Guid environmentId)
    {
        //create 2 years of data
        for (int i = 0; i <= 23; i++)
        {
            var documentMonthDate = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(-i);
            var currentCreationDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var documentIdentifier = new DocumentIdentifier(environmentId, documentMonthDate);

            var usageEntity = new UsageEntity
            {
                id = documentIdentifier.Value,
                PartitionKey = environmentId,
                ProjectId = projectId,
                EnvironmentId = environmentId,
                DocumentCreationDate = currentCreationDate,
                TotalMonthlyBandwidth = 0,
                TotalMonthlyMedia = 0,
                Days = new Dictionary<DateOnly, DailyUsageEntity>()
            };

            var daysInMonth = DateTime.DaysInMonth(documentMonthDate.Year, documentMonthDate.Month);
            var random = new Random();

            for (int j = 0; j < daysInMonth; j++)
            {
                var dailyDate = new DateOnly(documentMonthDate.Year, documentMonthDate.Month, j + 1);

                var dailyUsageEntity = new DailyUsageEntity
                {
                    BandwidthInBytes = 2048,
                    ContentNodes = 23,
                    Hostnames = 3,
                    MediaSizeInBytes = random.NextInt64(0, 1073741824) // Random long between 0 and 1GB
                };

                usageEntity.Days[dailyDate] = dailyUsageEntity;
                usageEntity.TotalMonthlyBandwidth += dailyUsageEntity.BandwidthInBytes;
                usageEntity.TotalMonthlyMedia += dailyUsageEntity.MediaSizeInBytes;
            }

            await CreateUsageDocument(usageEntity);
        }
    }
    #endregion
}