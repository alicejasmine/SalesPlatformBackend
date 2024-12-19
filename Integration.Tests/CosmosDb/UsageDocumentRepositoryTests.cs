using Domain.Entities;
using Infrastructure.Repositories.Usage;
using Integration.Tests.Library.TestContainers;
using Microsoft.Azure.Cosmos;
using Test.Fixtures.Usage;

namespace Integration.Tests.CosmosDb;

internal sealed class UsageDocumentRepositoryTests : CosmosDbTestFixture
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
        var usageEntity = UsageEntityFixture.DefaultUsage;

        //Act
        await _usageDocumentRepository.CreateUsageDocument(usageEntity);

        //Assert
        var usages = UsageTestContainer.GetItemLinqQueryable<UsageEntity>(allowSynchronousQueryExecution: true).ToList();
            
        Assert.That(usages, Is.Not.Null);
        Assert.That(usages.Count, Is.EqualTo(1));
            
        var storedUsageEntity = usages.First();
        Assert.That(storedUsageEntity.ProjectId, Is.EqualTo(UsageEntityFixture.DefaultUsage.ProjectId));
        Assert.That(storedUsageEntity.EnvironmentId, Is.EqualTo(UsageEntityFixture.DefaultUsage.EnvironmentId));
        Assert.That(storedUsageEntity.DocumentCreationDate, Is.EqualTo(UsageEntityFixture.DefaultUsage.DocumentCreationDate));
        Assert.That(storedUsageEntity.TotalMonthlyBandwidth, Is.EqualTo(UsageEntityFixture.DefaultUsage.TotalMonthlyBandwidth));
        Assert.That(storedUsageEntity.id, Is.EqualTo(UsageEntityFixture.DefaultUsage.id));
    }
    
    [Test]
    public async Task GetUsageEntity_ReturnsUsageEntity_WhenFound()
    {
        //Arrange
        var documentIdentifier = UsageEntityFixture.DefaultDocumentIdentifier;
        var usageEntity = UsageEntityFixture.DefaultUsage;
        await UsageTestContainer.CreateItemAsync(usageEntity, new PartitionKey(documentIdentifier.EnvironmentId.ToString()));
        
        //Act
        var usages = await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
        
        //Assert
        Assert.That(usages, Is.Not.Null);
        Assert.That(usages.Days.Count, Is.EqualTo(UsageEntityFixture.DefaultUsage.Days.Count));
        Assert.That(usages.id, Is.EqualTo(UsageEntityFixture.DefaultUsage.id));
        Assert.That(usages.PartitionKey, Is.EqualTo(UsageEntityFixture.DefaultUsage.PartitionKey));
    }
    
    [Test]
    public async Task GetUsageEntity_ReturnsNull_WhenNotFound()
    {
        //Arrange
        var documentIdentifier = UsageEntityFixture.DefaultDocumentIdentifier;
        
        //Act
        var usage = await _usageDocumentRepository.GetUsageEntity(documentIdentifier);
        
        //Assert
        Assert.That(usage, Is.Null);
    }
}