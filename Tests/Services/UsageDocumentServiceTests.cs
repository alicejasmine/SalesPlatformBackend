using ApplicationServices.Usage;
using Infrastructure.Repositories.Usage;
using Moq;
using Domain.ValueObject;
using Domain.Entities;
using Infrastructure.Repositories.Project;
using TestFixtures.Project;
using Test.Fixtures.Usage;
using TestFixtures.Usage;

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
    public async Task GetUsageEntitiesForMultipleMonths_ReturnsData_ForMultipleMonths()
    {
        // Arrange
        var project = ProjectEntityFixture.DefaultProject;
        var baseDate = new DateOnly(2024, 11, 1);
        var monthsToTake = 3;

        var usageEntities = new List<UsageEntity>
        {
            new UsageEntityBuilder()
                .WithEnvironmentId(project.EnvironmentId, baseDate)
                .WithDate(baseDate)
                .WithTotalMonthlyBandwidth(101)
                .WithTotalMonthlyMedia(201)
                .Build(),
            new UsageEntityBuilder()
                .WithEnvironmentId(project.EnvironmentId, baseDate)
                .WithDate(baseDate.AddMonths(-1))
                .WithTotalMonthlyBandwidth(102)
                .WithTotalMonthlyMedia(202)
                .Build(),
            new UsageEntityBuilder()
                .WithEnvironmentId(project.EnvironmentId, baseDate)
                .WithDate(baseDate.AddMonths(-2))
                .WithTotalMonthlyBandwidth(103)
                .WithTotalMonthlyMedia(203)
                .Build(),
        };

        foreach (var entity in usageEntities)
        {
            _usageDocumentRepository
                .Setup(x => x.GetUsageEntity(It.Is<DocumentIdentifier>(d => d.Date == entity.DocumentCreationDate)))
                .ReturnsAsync(entity);
        }

        // Act
        var result = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(project.Alias, baseDate.Month, baseDate.Year, monthsToTake);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(monthsToTake));
        Assert.That(result.Sum(u => u.TotalMonthlyBandwidth), Is.EqualTo(306));
        Assert.That(result.Sum(u => u.TotalMonthlyMedia), Is.EqualTo(606));
    }

    [Test]
    public async Task GetUsageEntitiesForMultipleMonths_ReturnsEmptyList_WhenNoDataExists()
    {
        // Arrange
        var project = ProjectEntityFixture.DefaultProject;
        var baseDate = new DateOnly(2024, 11, 1);
        var monthsToTake = 6;

        _projectRepository
            .Setup(x => x.GetEnvironmentIdByAlias(It.Is<string>(s => s == project.Alias)))
            .ReturnsAsync(project.EnvironmentId);

        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.IsAny<DocumentIdentifier>()))
            .ReturnsAsync((UsageEntity?)null);

        // Act
        var result = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(project.Alias, baseDate.Month, baseDate.Year, monthsToTake);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetUsageEntitiesForMultipleMonths_ReturnsPartialData_WhenSomeMonthsHaveNoData()
    {
        // Arrange
        var project = ProjectEntityFixture.DefaultProject;
        var baseDate = new DateOnly(2024, 11, 1);
        var monthsToTake = 6;

        var usageEntities = new List<UsageEntity>
        {
            new UsageEntityBuilder()
                .WithEnvironmentId(project.EnvironmentId, baseDate)
                .WithDate(baseDate)
                .Build(),
            new UsageEntityBuilder()
                .WithEnvironmentId(project.EnvironmentId, baseDate)
                .WithDate(baseDate.AddMonths(-1))
                .Build(),
            new UsageEntityBuilder()
                .WithEnvironmentId(project.EnvironmentId, baseDate)
                .WithDate(baseDate.AddMonths(-2))
                .Build(),
        };

        foreach (var entity in usageEntities)
        {
            _usageDocumentRepository
                .Setup(x => x.GetUsageEntity(It.Is<DocumentIdentifier>(d => d.Date == entity.DocumentCreationDate)))
                .ReturnsAsync(entity);
        }

        _projectRepository
            .Setup(x => x.GetEnvironmentIdByAlias(It.Is<string>(s => s == project.Alias)))
            .ReturnsAsync(project.EnvironmentId);

        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.Is<DocumentIdentifier>(d => d.Date == baseDate.AddMonths(-1))))
                    .ReturnsAsync((UsageEntity?)null);

        // Act
        var result = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(project.Alias, baseDate.Month, baseDate.Year, monthsToTake);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.Sum(u => u.TotalMonthlyBandwidth), Is.EqualTo(2048));
        Assert.That(result.Sum(u => u.TotalMonthlyMedia), Is.EqualTo(2048));
    }

    [Test]
    public async Task GetUsageEntity_ReturnsUsageEntity_WhenFound()
    {
        // Arrange
        var usageEntity = UsageEntityFixture.DefaultUsage;
        var project = ProjectEntityFixture.DefaultProject;
        var date = usageEntity.DocumentCreationDate;

        _projectRepository
            .Setup(x => x.GetEnvironmentIdByAlias(It.Is<string>(s => s == project.Alias)))
            .ReturnsAsync(usageEntity.EnvironmentId);

        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.Is<DocumentIdentifier>(d => d.EnvironmentId == usageEntity.EnvironmentId)))
            .ReturnsAsync(usageEntity);

        // Act
        var result = await _usageDocumentService.GetUsageEntity(project.Alias, date.Month, date.Year);


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
        var project = ProjectEntityFixture.DefaultProject;
        var defualtDate = UsageEntityFixture.DefaultUsage.DocumentCreationDate;

        _projectRepository
            .Setup(x => x.GetEnvironmentIdByAlias(It.Is<string>(s => s == project.Alias)))
            .ReturnsAsync(project.EnvironmentId);

        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.IsAny<DocumentIdentifier>()))
            .ReturnsAsync((UsageEntity?)null);

        // Act
        var result = await _usageDocumentService.GetUsageEntity(project.Alias, defualtDate.Month, defualtDate.Year);

        // Assert
        Assert.That(result, Is.Null);
        _usageDocumentRepository.Verify(
            x => x.GetUsageEntity(It.IsAny<DocumentIdentifier>()),
            Times.Once, "Repository method was not called."
        );
    }

    [Test]
    public async Task GetYearOfUsageData_ReturnsAggregatedData_WhenDataExists()
    {
        var project = ProjectEntityFixture.DefaultProject;
        var defualtDate = UsageEntityFixture.DefaultUsage.DocumentCreationDate;
        var usageEntities = new List<UsageEntity>();

        for (var i = 0; i < 12; i++)
        {
            usageEntities.Add(new UsageEntityBuilder()
                .WithEnvironmentId(project.EnvironmentId, defualtDate.AddMonths(-i))
                .WithDate(defualtDate.AddMonths(-i))
                .Build());
        }

        _projectRepository
            .Setup(x => x.GetEnvironmentIdByAlias(It.Is<string>(s => s == project.Alias)))
            .ReturnsAsync(project.EnvironmentId);

        foreach (var entity in usageEntities)
        {
            _usageDocumentRepository
                .Setup(x => x.GetUsageEntity(It.Is<DocumentIdentifier>(d => d.Date == entity.DocumentCreationDate)))
                .ReturnsAsync(entity);
        }

        // Act
        var (totalBandwidth, totalMedia) = await _usageDocumentService.GetYearOfUsageData(project.Alias, defualtDate.Month, defualtDate.Year);

        // Assert
        Assert.That(totalBandwidth, Is.EqualTo(12288));
        Assert.That(totalMedia, Is.EqualTo(12288));
    }

    [Test]
    public async Task GetYearOfUsageData_ReturnsZeros_WhenNoDataExists()
    {
        // Arrange
        var project = ProjectEntityFixture.DefaultProject;
        var defualtDate = UsageEntityFixture.DefaultUsage.DocumentCreationDate;

        _projectRepository
            .Setup(x => x.GetEnvironmentIdByAlias(It.Is<string>(s => s == project.Alias)))
            .ReturnsAsync(project.EnvironmentId);

        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.IsAny<DocumentIdentifier>()))
            .ReturnsAsync((UsageEntity?)null);

        // Act
        var (totalBandwidth, totalMedia) = await _usageDocumentService.GetYearOfUsageData(project.Alias, defualtDate.Month, defualtDate.Year);

        // Assert
        Assert.That(totalBandwidth, Is.EqualTo(0));
        Assert.That(totalMedia, Is.EqualTo(0));
    }
}