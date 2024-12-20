namespace Api.Service.Usage.DTOs
{
    public class TotalUsageResponse
    {
        public List<long> TotalDurationBandwidth { get; set; } = new();
        public List<long> TotalDurationMedia { get; set; } = new();
    }
}
