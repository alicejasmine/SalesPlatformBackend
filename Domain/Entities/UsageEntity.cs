namespace Domain.Entities;

public class UsageEntity
{
    public string id { get; init; }
    public Guid PartitionKey { get; init; }
    public Guid ProjectId { get; init; }
    public Guid EnvironmentId { get; init; }
    public DateOnly DocumentCreationDate { get; init; }
    public long TotalMonthlyBandwidth { get; set; }
    public long TotalMonthlyMedia { get; set; }
    public Dictionary<DateOnly, DailyUsageEntity> Days { get; set; } = new Dictionary<DateOnly, DailyUsageEntity>();
}

public class DailyUsageEntity
{
    public long BandwidthInBytes { get; set; }
    public int ContentNodes { get; set; }
    public int Hostnames { get; set; }
    public long MediaSizeInBytes { get; set; }
}