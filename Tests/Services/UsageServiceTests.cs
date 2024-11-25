using ApplicationServices.Usage;
using Infrastructure.Repositories.Usage;
using Moq;
using Microsoft.Extensions.Logging;
using Domain.ValueObject;
using Domain.Entities;

namespace Tests.Services;
public class UsageDocumentServiceTests
{
    private UsageDocumentService _usageDocumentService;
    private Mock<IUsageDocumentRepository> _usageDocumentRepository;


    [SetUp]
    public void Setup()
    {
        _usageDocumentRepository = new Mock<IUsageDocumentRepository>();

        _usageDocumentService = new UsageDocumentService(
            _usageDocumentRepository.Object
        );
    }

    [Test]
    public async Task GetUsageEntities_ReturnsData_ForMultipleMonths()
    {
        // Arrange
        var environmentID = Guid.NewGuid();
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
        var result = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(environmentID, baseDate.Month, baseDate.Year, monthsToTake);

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
        var environmentID = Guid.NewGuid();
        var baseDate = new DateOnly(2024, 11, 1);
        var monthsToTake = 6;

        _usageDocumentRepository
            .Setup(x => x.GetUsageEntity(It.IsAny<DocumentIdentifier>()))
            .ReturnsAsync((UsageEntity?)null);

        // Act
        var result = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(environmentID, baseDate.Month, baseDate.Year, monthsToTake);

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
        var result = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(environmentID, baseDate.Month, baseDate.Year, monthsToTake);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.Sum(u => u.TotalMonthlyBandwidth), Is.EqualTo(220));
        Assert.That(result.Sum(u => u.TotalMonthlyMedia), Is.EqualTo(420));
    }
}