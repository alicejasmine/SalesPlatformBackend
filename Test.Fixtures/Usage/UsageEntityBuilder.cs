using Domain.Entities;
using Domain.ValueObject;
using Test.Fixtures.Usage;

namespace TestFixtures.Usage;
public class UsageEntityBuilder
{
    private string _documentId = UsageEntityFixture.DefaultUsage.id;
    private Guid _partitionKey = UsageEntityFixture.DefaultUsage.PartitionKey;
    private Guid _projectId = UsageEntityFixture.DefaultUsage.ProjectId;
    private Guid _environmentId = UsageEntityFixture.DefaultUsage.EnvironmentId;
    private DateOnly _date = UsageEntityFixture.DefaultUsage.DocumentCreationDate;
    private long _totalMonthlyBandwidth = UsageEntityFixture.DefaultUsage.TotalMonthlyBandwidth;
    private long _totalMonthlyMedia = UsageEntityFixture.DefaultUsage.TotalMonthlyMedia;
    private Dictionary<DateOnly, DailyUsageEntity> _days = new Dictionary<DateOnly, DailyUsageEntity>(UsageEntityFixture.DefaultUsage.Days);

    public UsageEntity Build() => new UsageEntity
    {
        id = _documentId,
        PartitionKey = _partitionKey,
        ProjectId = _projectId,
        EnvironmentId = _environmentId,
        DocumentCreationDate = _date,
        TotalMonthlyBandwidth = _totalMonthlyBandwidth,
        TotalMonthlyMedia = _totalMonthlyMedia,
        Days = new Dictionary<DateOnly, DailyUsageEntity>(_days)
    };

    public UsageEntityBuilder WithEnvironmentId(Guid environmentId, DateOnly date)
    {
        _documentId = new DocumentIdentifier(environmentId, date).Value;
        _partitionKey = environmentId;
        _environmentId = environmentId;
        return this;
    }

    public UsageEntityBuilder WithProjectId(Guid projectId)
    {
        _projectId = projectId;
        return this;
    }

    public UsageEntityBuilder WithDate(DateOnly date)
    {
        _date = date;
        return this;
    }

    public UsageEntityBuilder WithTotalMonthlyBandwidth(long totalMonthlyBandwidth)
    {
        _totalMonthlyBandwidth = totalMonthlyBandwidth;
        return this;
    }

    public UsageEntityBuilder WithTotalMonthlyMedia(int totalMonthlyMedia)
    {
        _totalMonthlyMedia = totalMonthlyMedia;
        return this;
    }

    public UsageEntityBuilder WithDays(Dictionary<DateOnly, DailyUsageEntity> days)
    {
        _days = new Dictionary<DateOnly, DailyUsageEntity>(days);
        return this;
    }
}