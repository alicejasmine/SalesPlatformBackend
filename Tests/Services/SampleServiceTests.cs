using Moq;
using ApplicationServices.Sample;
using Infrastructure.Repository.Sample;
using Domain.Sample;
using System.Collections.Immutable;
using Test.Fixtures.Sample;

namespace Tests.Services;
internal class SampleServiceTests
{
    private SampleService _sampleService;

    private Mock<ISampleRepository> _sampleRepositoryMock;

    [SetUp]
    public void setup()
    {
        _sampleRepositoryMock = new Mock<ISampleRepository>();

        _sampleService = new SampleService(_sampleRepositoryMock.Object);
    }

    [Test]
    public async Task GetSampleByIdAsync_ReturnsSample_WhenSampleExists()
    {
        // Arrange
        var sampleModel = SampleModelFixture.DefaultSample;

        _sampleRepositoryMock.Setup(repo => repo.GetByIdAsync(sampleModel.Id))
                             .ReturnsAsync(sampleModel);

        // Act
        var result = await _sampleService.GetSampleByIdAsync(sampleModel.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(SampleModelFixture.DefaultSample.Id));
        Assert.That(result?.Name, Is.EqualTo(SampleModelFixture.DefaultSample.Name));
        _sampleRepositoryMock.Verify(repo => repo.GetByIdAsync(SampleModelFixture.DefaultSample.Id), Times.Once);
    }

    [Test]
    public async Task GetSampleByIdAsync_ReturnsNull_WhenSampleDoesNotExist()
    {
        // Arrange
        _sampleRepositoryMock.Setup(repo => repo.GetByIdAsync(SampleModelFixture.DefaultSample.Id))
                             .ReturnsAsync((SampleModel?)null);

        // Act
        var result = await _sampleService.GetSampleByIdAsync(SampleModelFixture.DefaultSample.Id);

        // Assert
        Assert.That(result, Is.Null);
        _sampleRepositoryMock.Verify(repo => repo.GetByIdAsync(SampleModelFixture.DefaultSample.Id), Times.Once);
    }

    [Test]
    public async Task GetAllSamplesAsync_ReturnsAllSamples()
    {
        // Arrange
        var sampleModels = new[]
        {
            SampleModelFixture.DefaultSample,
            new SampleModelBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Another Sample")
                .WithDescription("Another Description")
                .WithPrice(1)
                .Build()
    };

        _sampleRepositoryMock.Setup(repo => repo.GetAllSamplesAsync())
                             .ReturnsAsync(sampleModels);

        // Act
        var result = await _sampleService.GetAllSamplesAsync();

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        var firstResult = result[0];
        Assert.That(firstResult.Name, Is.EqualTo(sampleModels[0].Name));
        Assert.That(firstResult.Description, Is.EqualTo(sampleModels[0].Description));
        Assert.That(firstResult.Price, Is.EqualTo(sampleModels[0].Price));
        var secondResult = result[1];
        Assert.That(secondResult.Name, Is.EqualTo(sampleModels[1].Name));
        Assert.That(secondResult.Description, Is.EqualTo(sampleModels[1].Description));
        Assert.That(secondResult.Price, Is.EqualTo(sampleModels[1].Price));

        _sampleRepositoryMock.Verify(repo => repo.GetAllSamplesAsync(), Times.Once);
    }


    [Test]
    public async Task CreateSampleAsync_CreatesSampleSuccessfully()
    {
        // Arrange
        var sampleDto = new SampleDto(SampleModelFixture.DefaultSample.Id, SampleModelFixture.DefaultSample.Name, SampleModelFixture.DefaultSample.Description, SampleModelFixture.DefaultSample.Price, DateTime.Now, DateTime.Now);

        // Act
        await _sampleService.CreateSampleAsync(sampleDto);

        // Assert
        _sampleRepositoryMock.Verify(repo => repo.UpsertAsync(It.Is<SampleModel>(
            model => model.Id == SampleModelFixture.DefaultSample.Id &&
                     model.Name == sampleDto.Name &&
                     model.Description == sampleDto.Description &&
                     model.Price == sampleDto.Price
        )), Times.Once);
    }

    [Test]
    public async Task DeleteSampleAsync_DeletesSampleSuccessfully()
    {
        // Act
        await _sampleService.DeleteSampleAsync(SampleModelFixture.DefaultSample.Id);

        // Assert
        _sampleRepositoryMock.Verify(repo => repo.DeleteAsync(SampleModelFixture.DefaultSample.Id), Times.Once);
    }
}