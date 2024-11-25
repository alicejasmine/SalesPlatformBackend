using ApplicationServices.Usage;
using Infrastructure.Repositories.Usage;
using Moq;
using Microsoft.Extensions.Logging;
using Domain.ValueObject;
using Integration.Tests;

namespace Tests.Services;
public class UsageServiceTests
{
    private UsageDocumentService _usageDocumentService;
    private Mock<IUsageDocumentRepository> _usageDocumentRepository;
    private Mock<ILogger<UsageDocumentService>> _loggerMock;


    [SetUp]
    public void Setup()
    {
        _usageDocumentRepository = new Mock<IUsageDocumentRepository>();
        _loggerMock = new Mock<ILogger<UsageDocumentService>>();

        _usageDocumentService = new UsageDocumentService(
            _usageDocumentRepository.Object,
            _loggerMock.Object
        );
    }

    [Test]
    public async Task GetUsageEntities_GetUsageEntities_WhenSucces()
    {
        // Arrange
        var documentIdentifier = UsageEntityFixture.DefaultDocumentIdentifier;
        var environmentID = UsageEntityFixture.DefaultUsage.EnvironmentId;
        var date = UsageEntityFixture.DefaultUsage.DocumentCreationDate;
        var monthsToTake = 6;
        //add to database
        _usageDocumentRepository
                .Setup(x => x.GetUsageEntity(It.IsAny<DocumentIdentifier>()))
                .ReturnsAsync(UsageEntityFixture.DefaultUsage);

        // Act
        var createdResponse = await _usageDocumentService.GetUsageEntities(environmentID, date.Month, date.Year, monthsToTake);

        // Assert
        Assert.That(createdResponse, Is.Not.Null);
        Assert.That(createdResponse.Count(), Is.EqualTo(monthsToTake));
        var firstItem = createdResponse.First();
        Assert.That(firstItem.DocumentCreationDate, Is.EqualTo(date));
        Assert.That(firstItem.EnvironmentId, Is.EqualTo(environmentID));
    }
}