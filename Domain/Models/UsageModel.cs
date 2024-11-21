namespace Domain.Models;

public class UsageModel
{
    public string DocumentId { get; init; }
    public Guid PartitionKey { get; init; }
    public Guid ProjectId { get; init; }
    public Guid EnvironmentId { get; init; }
    public DateOnly CreationDate { get; init; }
    public long TotalMonthlyBandwidth { get; set; }
    public long TotalMonthlyMedia { get; set; }
    public Dictionary<DateOnly, DailyUsageModel> Days { get; init; } = new Dictionary<DateOnly, DailyUsageModel>();
}