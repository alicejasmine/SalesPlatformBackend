using Moq;
using ApplicationServices.Sample;
using Infrastructure.Repository.Sample;
using Domain.Sample;
using System.Collections.Immutable;

namespace Tests.Services;
internal class SampleServiceTests
{
    private SampleService _sampleService;

    private Mock<ISampleRepository> _sampleRepositoryMock;

    private static readonly Guid _id = Guid.NewGuid();

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
        var sampleModel = new SampleModel(_id, "Sample Name", "Description", 100, DateTime.Now, DateTime.Now);
        _sampleRepositoryMock.Setup(repo => repo.GetSampleEntityByIdAsync(_id))
                             .ReturnsAsync(sampleModel);

        // Act
        var result = await _sampleService.GetSampleByIdAsync(_id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(_id));
        Assert.That(result?.Name, Is.EqualTo("Sample Name"));
        _sampleRepositoryMock.Verify(repo => repo.GetSampleEntityByIdAsync(_id), Times.Once);
    }

    [Test]
    public async Task GetSampleByIdAsync_ReturnsNull_WhenSampleDoesNotExist()
    {
        // Arrange
        _sampleRepositoryMock.Setup(repo => repo.GetSampleEntityByIdAsync(_id))
                             .ReturnsAsync((SampleModel?)null);

        // Act
        var result = await _sampleService.GetSampleByIdAsync(_id);

        // Assert
        Assert.That(result, Is.Null);
        _sampleRepositoryMock.Verify(repo => repo.GetSampleEntityByIdAsync(_id), Times.Once);
    }

    [Test]
    public async Task GetAllSamplesAsync_ReturnsAllSamples()
    {
        // Arrange
        var sampleModels = new[]
        {
        new SampleModel(Guid.NewGuid(), "Sample1", "Description1", 50, DateTime.Now, DateTime.Now),
        new SampleModel(Guid.NewGuid(), "Sample2", "Description2", 150, DateTime.Now, DateTime.Now)
    };

        _sampleRepositoryMock.Setup(repo => repo.GetAllSamplesAsync())
                             .ReturnsAsync(sampleModels);

        // Act
        var result = await _sampleService.GetAllSamplesAsync();

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));

        Assert.That(result[0].Name, Is.EqualTo("Sample1"));
        Assert.That(result[0].Description, Is.EqualTo("Description1"));
        Assert.That(result[0].Price, Is.EqualTo(50));
        Assert.That(result[1].Name, Is.EqualTo("Sample2"));
        Assert.That(result[1].Description, Is.EqualTo("Description2"));
        Assert.That(result[1].Price, Is.EqualTo(150));

        _sampleRepositoryMock.Verify(repo => repo.GetAllSamplesAsync(), Times.Once);
    }


    [Test]
    public async Task CreateSampleAsync_CreatesSampleSuccessfully()
    {
        // Arrange
        var sampleDto = new SampleDto(_id, "New Sample", "New Description", 200, DateTime.Now, DateTime.Now);

        // Act
        await _sampleService.CreateSampleAsync(sampleDto);

        // Assert
        _sampleRepositoryMock.Verify(repo => repo.UpsertAsync(It.Is<SampleModel>(
            model => model.Id == _id &&
                     model.Name == sampleDto.Name &&
                     model.Description == sampleDto.Description &&
                     model.Price == sampleDto.Price
        )), Times.Once);
    }

    [Test]
    public async Task DeleteSampleAsync_DeletesSampleSuccessfully()
    {
        // Act
        await _sampleService.DeleteSampleAsync(_id);

        // Assert
        _sampleRepositoryMock.Verify(repo => repo.DeleteAsync(_id), Times.Once);
    }
}