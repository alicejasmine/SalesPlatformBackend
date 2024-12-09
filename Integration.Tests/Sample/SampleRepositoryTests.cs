using Domain.Sample;
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
        var anotherSampleEntity = new SampleModelBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Another Sample")
            .WithDescription("Another Description")
            .WithPrice(1)
            .Build();

        await _sampleRepository.UpsertAsync(sampleEntity);
        await _sampleRepository.UpsertAsync(anotherSampleEntity);

        // Act
        var samples = await _sampleRepository.GetAllSamplesAsync();

        // Assert
        Assert.That(samples, Is.Not.Empty);
        Assert.That(samples.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllSamples_ShouldReturnEmpty_WhenNoSamplesExist()
    {
        // Act
        var samples = await _sampleRepository.GetAllSamplesAsync();

        // Assert
        Assert.That(samples, Is.Empty);
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
        Assert.That(sample.Id, Is.EqualTo(SampleModelFixture.DefaultSample.Id));
        Assert.That(sample.Name, Is.EqualTo(SampleModelFixture.DefaultSample.Name));
        Assert.That(sample.Description, Is.EqualTo(SampleModelFixture.DefaultSample.Description));
        Assert.That(sample.Price, Is.EqualTo(SampleModelFixture.DefaultSample.Price));
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
    public async Task UpsertAsync_ShouldInsertNewSample()
    {
        // Arrange
        var sampleEntity = SampleModelFixture.DefaultSample;

        // Act
        var insertedSample = await _sampleRepository.UpsertAsync(sampleEntity);

        // Assert
        Assert.That(DatabaseTestsFixture.DbContext.SampleEntities.Count, Is.EqualTo(1));
        Assert.That(insertedSample, Is.Not.Null);
        Assert.That(insertedSample.Id, Is.EqualTo(SampleModelFixture.DefaultSample.Id));
        Assert.That(insertedSample.Name, Is.EqualTo(SampleModelFixture.DefaultSample.Name));
        Assert.That(insertedSample.Description, Is.EqualTo(SampleModelFixture.DefaultSample.Description));
        Assert.That(insertedSample.Price, Is.EqualTo(SampleModelFixture.DefaultSample.Price));
    }

    [Test]
    public async Task UpsertAsync_ShouldUpdateExistingSample()
    {
        // Arrange
        var sampleEntity = SampleModelFixture.DefaultSample;
        var insertedSample = await _sampleRepository.UpsertAsync(sampleEntity);

        Assert.That(DatabaseTestsFixture.DbContext.SampleEntities.Count, Is.EqualTo(1));
        insertedSample.Name = "Updated Product Name";

        // Act
        var updatedSample = await _sampleRepository.UpsertAsync(insertedSample);

        // Assert
        Assert.That(updatedSample, Is.Not.Null);
        Assert.That(updatedSample.Name, Is.EqualTo("Updated Product Name"));
    }

    [Test]
    public async Task DeleteAsync_ShouldRemoveSample()
    {
        // Arrange
        var sampleEntity = SampleModelFixture.DefaultSample;
        await _sampleRepository.UpsertAsync(sampleEntity);

        // Act
        await _sampleRepository.DeleteAsync(sampleEntity.Id);

        // Assert
        var sample = await _sampleRepository.GetByIdAsync(sampleEntity.Id);
        Assert.That(sample, Is.Null);
    }

    [Test]
    public void DeleteAsync_ShouldThrowException_WhenSampleDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _sampleRepository.DeleteAsync(nonExistingId));
    }
}