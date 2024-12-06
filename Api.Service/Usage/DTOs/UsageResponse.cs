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
        public long BandwidthInBytes { get; set; }
        public int ContentNodes { get; set; }
        public int Hostnames { get; set; }
        public long MediaSizeInBytes { get; set; }
    }
}