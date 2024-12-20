using Domain.Models;
using Domain.ValueObject;
using Test.Fixtures.Usage;

namespace TestFixtures.Usage
{
    internal class UsageModelBuilder
    {
        private string _documentId = UsageModelFixture.DefaultUsage.DocumentId;
        private Guid _partitionKey = UsageModelFixture.DefaultUsage.PartitionKey;
        private Guid _projectId = UsageModelFixture.DefaultUsage.ProjectId;
        private Guid _environmentId = UsageModelFixture.DefaultUsage.EnvironmentId;
        private DateOnly _date = Test.Fixtures.Usage.UsageEntityFixture.DefaultUsage.DocumentCreationDate;
        private long _totalMonthlyBandwidth = UsageModelFixture.DefaultUsage.TotalMonthlyBandwidth;
        private long _totalMonthlyMedia = UsageModelFixture.DefaultUsage.TotalMonthlyMedia;
        private Dictionary<DateOnly, DailyUsageModel> _days = new Dictionary<DateOnly, DailyUsageModel>(UsageModelFixture.DefaultUsage.Days);

        public UsageModel Build() => new UsageModel
        {
            DocumentId = _documentId,
            PartitionKey = _partitionKey,
            ProjectId = _projectId,
            EnvironmentId = _environmentId,
            CreationDate = _date,
            TotalMonthlyBandwidth = _totalMonthlyBandwidth,
            TotalMonthlyMedia = _totalMonthlyMedia,
            Days = new Dictionary<DateOnly, DailyUsageModel>(_days)
        };

        public UsageModelBuilder WithId(Guid environmentId, DateOnly date)
        {
            _documentId = new DocumentIdentifier(environmentId, date).Value;
            return this;
        }
    }
}