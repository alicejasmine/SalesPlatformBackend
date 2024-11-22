namespace Api.Service.DTOs;

public class UsageResponse
{
    public Guid EnvironmentId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public long TotalMonthlyBandwidth { get; set; }
    public long TotalMonthlyMedia { get; set; }
    public List<DailyUsageResponse> DailyUsages { get; set; }


    public class DailyUsageResponse
    {
        public DateOnly Date { get; set; }
        public BandwidthResponse Bandwidth { get; set; }
        public long MediaSizeInBytes { get; set; }
    }

    public class BandwidthResponse
    {
        public long TotalBytes { get; set; }
        public int RequestCount { get; set; }
    }
}