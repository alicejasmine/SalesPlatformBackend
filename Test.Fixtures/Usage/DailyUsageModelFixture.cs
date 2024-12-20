using Domain.Models;

namespace TestFixtures.Usage;

public static class DailyUsageModelFixture
{
    public static DailyUsageModel DefaultDailyUsage { get; } = new DailyUsageModel
    {
        BandwidthInBytes = 1024,
        Hostnames = 12,
        ContentNodes = 65,
        MediaSizeInBytes = 2048
    };
}