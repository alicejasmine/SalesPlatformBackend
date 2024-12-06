using Domain.Sample;
using Infrastructure.Repository.Sample;
using Integration.Tests.Library;

namespace Integration.Tests.Sample;

[TestFixture]
internal class SampleRepositoryTests : BaseRepositoryTests<SampleRepository>
{
    protected override SampleRepository CreateRepository() => new(DbContext);

    [Test]
    public async Task GetAllSamples_ShouldReturnAllSamples()
    {
        // Arrange
        var sampleEntity = new SampleEntity(
            Guid.NewGuid(),
            "Test Product",
            "Description of test product",
            100,
            DateTime.Now,
            DateTime.Now
        );

        DbContext.SampleEntities.Add(sampleEntity);
        await DbContext.SaveChangesAsync();

        // Act
        var samples = await Repository.GetAllSamplesAsync();

        // Assert
        Assert.That(samples, Is.Not.Empty);
        Assert.That(samples.First().Name, Is.EqualTo("Test Product"));
    }

    [Test]
    public async Task GetSampleById_ShouldReturnCorrectSample()
    {
        // Arrange
        var sampleEntity = new SampleEntity(
            Guid.NewGuid(),
            "Sample Product",
            "Description of sample product",
            150,
            DateTime.Now,
            DateTime.Now
        );

        DbContext.SampleEntities.Add(sampleEntity);
        await DbContext.SaveChangesAsync();

        // Act
        var sample = await Repository.GetByIdAsync(sampleEntity.Id);

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
        var sample = await Repository.GetByIdAsync(nonExistingId);

        // Assert
        Assert.That(sample, Is.Null);
    }

    [Test]
    public async Task GetAllSamples_ShouldReturnEmpty_WhenNoSamplesExist()
    {
        // Act
        var samples = await Repository.GetAllSamplesAsync();

        // Assert
        Assert.That(samples, Is.Empty);
    }
}