using System.Runtime.InteropServices.JavaScript;
using Domain.Entities;
using Domain.ValueObject;
using Infrastructure.Repositories.Usage;
using Microsoft.Azure.Cosmos;


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
    
        
    [Test]
    public async Task GetUsageEntity_ReturnsUsageEntity_WhenSuccess()
    {
        //Arrange
        var documentIdentifier = UsageEntityFixture.DefaultDocumentIdentifier;
        var usageEntity = UsageEntityFixtures.DefaultUsage;
        await UsageTestContainer.CreateItemAsync(usageEntity, new PartitionKey(documentIdentifier.EnvironmentId.ToString()));
        
        //Act
        var usages = await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
        
        //Assert
        Assert.That(usages, Is.Not.Null);
        Assert.That(usages.Days.Count, Is.EqualTo(UsageEntityFixture.DefaultUsage.Days.Count));
        Assert.That(usages.id, Is.EqualTo(UsageEntityFixture.DefaultUsage.id));
        Assert.That(usages.PartitionKey, Is.EqualTo(UsageEntityFixture.DefaultUsage.PartitionKey));
    }
}