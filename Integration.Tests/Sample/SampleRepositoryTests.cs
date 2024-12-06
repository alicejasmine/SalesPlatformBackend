using Infrastructure.Repository.Sample;
using Integration.Tests.Library;
using Test.Fixtures.Sample;

namespace Integration.Tests.Sample;

[TestFixture]
public sealed class SampleRepositoryTests : BaseDatabaseTestFixture
{
    private SampleRepository _sampleRepository;

    [SetUp]
    public async Task SetupTest()
    {
        _sampleRepository = new(DatabaseTestsFixture.DbContext);
    }

    [Test]
    public async Task GetAllSamples_ShouldReturnAllSamples()
    {
        // Arrange
        var sampleEntity = SampleModelFixture.DefaultSample;

        await _sampleRepository.UpsertAsync(sampleEntity);

        // Act
        var samples = await _sampleRepository.GetAllSamplesAsync();

        // Assert
        Assert.That(samples, Is.Not.Empty);
        Assert.That(samples.First().Name, Is.EqualTo("Test Product"));
    }

    [Test]
    public async Task GetSampleById_ShouldReturnCorrectSample()
    {
        // Arrange
        var sampleEntity = SampleModelFixture.DefaultSample;

        await _sampleRepository.UpsertAsync(sampleEntity);

        // Act
        var sample = await _sampleRepository.GetByIdAsync(sampleEntity.Id);

        // Assert
        Assert.That(sample, Is.Not.Null);
        Assert.That(sample.Name, Is.EqualTo("Sample Product"));
    }

    [Test]
    public async Task GetSampleById_ShouldReturnNull_WhenSampleDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var sample = await _sampleRepository.GetByIdAsync(nonExistingId);

        // Assert
        Assert.That(sample, Is.Null);
    }

    [Test]
    public async Task GetAllSamples_ShouldReturnEmpty_WhenNoSamplesExist()
    {
        // Act
        var samples = await _sampleRepository.GetAllSamplesAsync();

        // Assert
        Assert.That(samples, Is.Empty);
    }
}