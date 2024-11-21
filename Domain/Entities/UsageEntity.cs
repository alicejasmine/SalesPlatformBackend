namespace Domain.Entities;

public class UsageEntity
{
    public string id { get; init; }
    public Guid PartitionKey { get; init; }
    public Guid ProjectId { get; init; }
    public Guid EnvironmentId { get; init; }
    public DateOnly DocumentCreationDate { get; init; }
    public BandwidthDataEntity TotalMonthlyBandwidth { get; set; }
    public long TotalMonthlyMedia { get; set; }
    public Dictionary<DateOnly, DailyUsageEntity> Days { get; set; } = new Dictionary<DateOnly, DailyUsageEntity>();
}

public class DailyUsageEntity
{
    public BandwidthDataEntity Bandwidth { get; set; }
    public long MediaSizeInBytes { get; set; }
}

public class BandwidthDataEntity
{
    public long TotalBytes { get; init; }
    public int RequestCount { get; init; }
}