using ApplicationServices.Usage;
using Infrastructure.Repositories.Usage;
using Moq;
using Domain.ValueObject;
using Domain.Entities;
using Test.Fixtures.Usage;
using Infrastructure.Repositories.Project;

namespace Tests.Services;
public class UsageDocumentServiceTests
{
    private UsageDocumentService _usageDocumentService;
    private Mock<IUsageDocumentRepository> _usageDocumentRepository;
    private Mock<IProjectRepository> _projectRepository;

    [SetUp]
    public void Setup()
    {
        _usageDocumentRepository = new Mock<IUsageDocumentRepository>();
        _projectRepository = new Mock<IProjectRepository>();

        _usageDocumentService = new UsageDocumentService(
            _usageDocumentRepository.Object, _projectRepository.Object
        );
    }

    [Test]
    public async Task GetUsageEntities_ReturnsData_ForMultipleMonths()
    {
        // Arrange
        var environmentID = Guid.NewGuid();
        var projectAlias = "oxygen-website1";
        var baseDate = new DateOnly(2024, 11, 1);
        var monthsToTake = 3;

        var usageEntities = new List<UsageEntity>
        {
            new UsageEntity { EnvironmentId = environmentID, DocumentCreationDate = baseDate, TotalMonthlyBandwidth = 100, TotalMonthlyMedia = 200 },
            new UsageEntity { EnvironmentId = environmentID, DocumentCreationDate = baseDate.AddMonths(-1), TotalMonthlyBandwidth = 150, TotalMonthlyMedia = 250 },
            new UsageEntity { EnvironmentId = environmentID, DocumentCreationDate = baseDate.AddMonths(-2), TotalMonthlyBandwidth = 120, TotalMonthlyMedia = 220 },
        };

        foreach (var entity in usageEntities)
        {
            _usageDocumentRepository
                .Setup(x => x.GetUsageEntity(It.Is<DocumentIdentifier>(d => d.Date == entity.DocumentCreationDate)))
                .ReturnsAsync(entity);
        }

        // Act
        var result = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(projectAlias, baseDate.Month, baseDate.Year, monthsToTake);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(monthsToTake));
        Assert.That(result.Sum(u => u.TotalMonthlyBandwidth), Is.EqualTo(370));
        Assert.That(result.Sum(u => u.TotalMonthlyMedia), Is.EqualTo(670));
    }

    [Test]
    public async Task GetUsageEntities_ReturnsEmptyList_WhenNoDataExists()
    {
        // Arrange
        var projectAlias = "oxygen-website1";
        var baseDate = new DateOnly(2024, 11, 1);
        var monthsToTake = 6;

        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.IsAny<DocumentIdentifier>()))
            .ReturnsAsync((UsageEntity?)null);

        // Act
        var result = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(projectAlias, baseDate.Month, baseDate.Year, monthsToTake);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task GetUsageEntities_ReturnsPartialData_WhenSomeMonthsHaveNoData()
    {
        // Arrange
        var environmentID = Guid.NewGuid();
        var baseDate = new DateOnly(2024, 11, 1);
        var monthsToTake = 6;
        var projectAlias = "oxygen-website1";
        
        var usageEntities = new List<UsageEntity>
    {
        new UsageEntity { EnvironmentId = environmentID, DocumentCreationDate = baseDate, TotalMonthlyBandwidth = 100, TotalMonthlyMedia = 200 },
        new UsageEntity { EnvironmentId = environmentID, DocumentCreationDate = baseDate.AddMonths(-2), TotalMonthlyBandwidth = 120, TotalMonthlyMedia = 220 },
    };

        foreach (var entity in usageEntities)
        {
            _usageDocumentRepository
                .Setup(x => x.GetUsageEntity(It.Is<DocumentIdentifier>(d => d.Date == entity.DocumentCreationDate)))
                .ReturnsAsync(entity);
        }

        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.Is<DocumentIdentifier>(d => d.Date == baseDate.AddMonths(-1))))
            .ReturnsAsync((UsageEntity?)null);

        // Act
        var result = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(projectAlias, baseDate.Month, baseDate.Year, monthsToTake);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.Sum(u => u.TotalMonthlyBandwidth), Is.EqualTo(220));
        Assert.That(result.Sum(u => u.TotalMonthlyMedia), Is.EqualTo(420));
    }
    
    [Test]
    public async Task GetUsageEntity_ReturnsUsageEntity_WhenFound()
    {
        // Arrange
        var usageEntity = UsageEntityFixture.DefaultUsage;
        var date = new DateOnly(2024, 11, 1);
        var projectAlias = "oxygen-website1";
        
        _projectRepository
            .Setup(x => x.GetEnvironmentIdByAlias(It.Is<string>(s => s == projectAlias)))
            .ReturnsAsync(usageEntity.EnvironmentId);
        
        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.Is<DocumentIdentifier>(d => d.EnvironmentId == usageEntity.EnvironmentId)))
            .ReturnsAsync(usageEntity);

        // Act
        var result = await _usageDocumentService.GetUsageEntity(projectAlias, date.Month, date.Year);
        
      
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.EnvironmentId, Is.EqualTo(usageEntity.EnvironmentId));
        Assert.That(result.ProjectId, Is.EqualTo(usageEntity.ProjectId));
        Assert.That(result.TotalMonthlyBandwidth, Is.EqualTo(usageEntity.TotalMonthlyBandwidth));
        Assert.That(result.TotalMonthlyMedia, Is.EqualTo(usageEntity.TotalMonthlyMedia));
        Assert.That(result.Days.Count, Is.EqualTo(usageEntity.Days.Count));
        
        foreach (var (key, value) in usageEntity.Days)
        {
            Assert.That(result.Days.ContainsKey(key), $"Result is missing day: {key}");
            var actualDailyUsage = result.Days[key];
            
            Assert.That(actualDailyUsage.MediaSizeInBytes, Is.EqualTo(value.MediaSizeInBytes), $"Mismatch in MediaSizeInBytes for day: {key}");
            Assert.That(actualDailyUsage.BandwidthInBytes, Is.EqualTo(value.BandwidthInBytes), $"Mismatch in BandwidthInBytes for day: {key}");
            Assert.That(actualDailyUsage.Hostnames, Is.EqualTo(value.Hostnames), $"Mismatch in Hostnames for day: {key}");
            Assert.That(actualDailyUsage.ContentNodes, Is.EqualTo(value.ContentNodes), $"Mismatch in ContentNodes for day: {key}");
        }
    }
    
    [Test]
    public async Task GetUsageEntity_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var environmentId = UsageEntityFixture.DefaultDocumentIdentifier.EnvironmentId;
        var date = new DateOnly(2024, 11, 1);
        var projectAlias = "oxygen-website1";

        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.IsAny<DocumentIdentifier>()))
            .ReturnsAsync((UsageEntity?)null); 

        // Act
        var result = await _usageDocumentService.GetUsageEntity(projectAlias, date.Month, date.Year);

        // Assert
        Assert.That(result, Is.Null);
        _usageDocumentRepository.Verify(
            x => x.GetUsageEntity(It.IsAny<DocumentIdentifier>()),
            Times.Once, "Repository method was not called."
        );
    }
}