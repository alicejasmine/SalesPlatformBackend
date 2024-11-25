using Domain.Entities;
using Infrastructure.Repositories.Usage;

namespace Integration.Tests.CosmosDb;

public class UsageDocumentRepositoryTests : CosmosDbTestFixture
{
    private UsageDocumentRepository _usageDocumentRepository;

    [SetUp]
    public override Task DoSetup()
    {
        _usageDocumentRepository = new UsageDocumentRepository(UsageTestContainer);
        
        return Task.CompletedTask;
    }
    
    [Test]
    public async Task CreateUsageDocument_DoesStoreUsageEntity_WhenSuccess()
    {
        //Arrange
        var usageEntity = UsageEntityFixtures.DefaultUsage;

        //Act
        await _usageDocumentRepository.CreateUsageDocument(usageEntity);

        //Assert
        var usages = UsageTestContainer.GetItemLinqQueryable<UsageEntity>(allowSynchronousQueryExecution: true).ToList();
            
        Assert.That(usages, Is.Not.Null);
        Assert.That(usages.Count, Is.EqualTo(1));
            
        var storedUsageEntity = usages.First();
        Assert.That(storedUsageEntity.ProjectId, Is.EqualTo(UsageEntityFixtures.DefaultUsage.ProjectId));
        Assert.That(storedUsageEntity.EnvironmentId, Is.EqualTo(UsageEntityFixtures.DefaultUsage.EnvironmentId));
        Assert.That(storedUsageEntity.DocumentCreationDate, Is.EqualTo(UsageEntityFixtures.DefaultUsage.DocumentCreationDate));
        Assert.That(storedUsageEntity.TotalMonthlyBandwidth, Is.EqualTo(UsageEntityFixtures.DefaultUsage.TotalMonthlyBandwidth));
        Assert.That(storedUsageEntity.id, Is.EqualTo(UsageEntityFixtures.DefaultUsage.id));
    }
}