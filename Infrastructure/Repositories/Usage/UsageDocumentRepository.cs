using Domain.Entities;
using Domain.ValueObject;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

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
                    Bandwidth = new BandwidthDataEntity
                    {
                        TotalBytes = random.NextInt64(0, 1073741824), // Random long between 0 and 1GB
                        RequestCount = random.Next(0, 10000)           // Random int between 0 and 10000
                    },
                    MediaSizeInBytes = random.NextInt64(0, 1073741824) // Random long between 0 and 1GB
                };

                usageEntity.Days[dailyDate] = dailyUsageEntity;
                usageEntity.TotalMonthlyBandwidth += dailyUsageEntity.Bandwidth.TotalBytes;
                usageEntity.TotalMonthlyMedia += dailyUsageEntity.MediaSizeInBytes;
            }
            //save usage entity to cosmo

            await CreateUsageDocument(usageEntity);
        }
    }
    #endregion
}