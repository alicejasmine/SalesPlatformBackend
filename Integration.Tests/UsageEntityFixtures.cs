using Domain.Entities;

namespace Integration.Tests;

public static class UsageEntityFixtures
{
    private static readonly Guid DefaultEnvironmentId = Guid.Parse("3b95c098-21b4-4f8b-9f7c-6a435ed83931");
    private static readonly DateOnly DefaultDate = DateOnly.Parse("2024-01-01");
    

    public static UsageEntity DefaultUsage { get; } = new UsageEntity
    {
        id = "d5476bbd-bd30-4f54-8a63-b1bc1a8e4bc2",
        PartitionKey = DefaultEnvironmentId,
        ProjectId = Guid.Parse("d5476bbd-bd30-4f54-8a63-b1bc1a8e4bc2"),
        EnvironmentId = DefaultEnvironmentId,
        DocumentCreationDate = DefaultDate,
        TotalMonthlyBandwidth = 1024,
        TotalMonthlyMedia= 1024,
        Days = new Dictionary<DateOnly, DailyUsageEntity>
        {
            { DefaultDate, DailyUsageEntityFixture.DefaultDailyUsage },
            { DateOnly.Parse("2024-01-02"), DailyUsageEntityFixture.DefaultDailyUsage }
        }
    };
    
    public static class DailyUsageEntityFixture
    {
        public static DailyUsageEntity DefaultDailyUsage { get; } = new DailyUsageEntity
        {
            Bandwidth = new BandwidthDataEntity { TotalBytes = 1024, RequestCount = 100 },
            MediaSizeInBytes = 2048,
        };
    }
}