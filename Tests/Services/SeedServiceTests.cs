using ApplicationServices.Seed;
using ApplicationServices.Usage;
using Infrastructure.Repositories.Usage;
using Moq;

namespace Tests.Services;
public class SeedServiceTests
{
    private SeedService _seedService;
    private Mock<IUsageDocumentRepository> _usageDocumentRepository;

    [SetUp]
    public void Setup()
    {
        _usageDocumentRepository = new Mock<IUsageDocumentRepository>();

        _seedService = new SeedService(
            _usageDocumentRepository.Object
        );
    }

    [Test]
    public async Task SeedUsageDocument_ShouldCallRepositoryCorrectNumberOfTimes_WhenSuccess()
    {
        // Arrange
        var expectedSeedCount = 5;
        var CapturesprojectIds = new List<Guid>();
        var CapturedEnvironmentIds = new List<Guid>();

        _usageDocumentRepository
            .Setup(repo => repo.SeedUsageDocument(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Callback<Guid, Guid>((projectId, environmentId) =>
            {
                CapturesprojectIds.Add(projectId);
                CapturedEnvironmentIds.Add(environmentId);
            })
            .Returns(Task.CompletedTask);

        // Act
        await _seedService.SeedDatabasesWithData();

        // Assert
        _usageDocumentRepository.Verify(
            repo => repo.SeedUsageDocument(It.IsAny<Guid>(), It.IsAny<Guid>()),
            Times.Exactly(expectedSeedCount)
        );

        Assert.That(CapturesprojectIds.Count, Is.EqualTo(expectedSeedCount));
        Assert.That(CapturedEnvironmentIds.Count, Is.EqualTo(expectedSeedCount));
        Assert.That(CapturesprojectIds, Is.Unique);
        Assert.That(CapturedEnvironmentIds, Is.Unique);
    }

    [Test]
    public async Task SeedUsageDocument_ShouldGenerateValidData_WhenSuccess()
    {
        // Arrange
        var CapturesprojectIds = new List<Guid>();
        var CapturedEnvironmentIds = new List<Guid>();

        _usageDocumentRepository
            .Setup(repo => repo.SeedUsageDocument(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Callback<Guid, Guid>((projectId, environmentId) =>
            {
                CapturesprojectIds.Add(projectId);
                CapturedEnvironmentIds.Add(environmentId);
            })
            .Returns(Task.CompletedTask);

        // Act
        await _seedService.SeedDatabasesWithData();

        // Assert
        Assert.That(CapturesprojectIds, Has.All.Not.EqualTo(Guid.Empty));
        Assert.That(CapturedEnvironmentIds, Has.All.Not.EqualTo(Guid.Empty));
        Assert.That(CapturesprojectIds, Is.Unique);
        Assert.That(CapturedEnvironmentIds, Is.Unique);
    }
}