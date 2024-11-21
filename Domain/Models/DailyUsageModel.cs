namespace Domain.Models;

public class DailyUsageModel
{
    public BandwidthDataModel Bandwidth { get; set; }
    public long MediaSizeInBytes { get; set; }
}

public class BandwidthDataModel
{
    public long TotalBytes { get; init; }
    public int RequestCount { get; init; }
}