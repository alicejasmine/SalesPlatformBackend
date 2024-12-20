namespace Domain.Models;

public class DailyUsageModel
{
    public long BandwidthInBytes { get; set; }
    public int ContentNodes { get; set; }
    public int Hostnames { get; set; }
    public long MediaSizeInBytes { get; set; }
}