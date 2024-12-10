using Domain.Models;
using Domain.ValueObject;

namespace TestFixtures.Usage;

public static class UsageModelFixture
{
        private static readonly Guid DefaultEnvironmentId = Guid.Parse("3b95c098-21b4-4f8b-9f7c-6a435ed83931");
        private static readonly DateOnly DefaultDate = DateOnly.Parse("2024-01-01");

        public static readonly DocumentIdentifier DefaultDocumentIdentifier = new DocumentIdentifier(DefaultEnvironmentId, DefaultDate);

        public static UsageModel DefaultUsage { get; } = new UsageModel
        {
            DocumentId = DefaultDocumentIdentifier.Value,
            PartitionKey = DefaultEnvironmentId,
            ProjectId = Guid.Parse("d5476bbd-bd30-4f54-8a63-b1bc1a8e4bc2"),
            EnvironmentId = DefaultEnvironmentId,
            CreationDate = DefaultDate,
            TotalMonthlyBandwidth = 2048,
            TotalMonthlyMedia = 4096,
            Days = new Dictionary<DateOnly, DailyUsageModel>
            {
                { DefaultDate, DailyUsageModelFixture.DefaultDailyUsage },
                { DateOnly.Parse("2024-01-02"), DailyUsageModelFixture.DefaultDailyUsage }
            }
        };
}