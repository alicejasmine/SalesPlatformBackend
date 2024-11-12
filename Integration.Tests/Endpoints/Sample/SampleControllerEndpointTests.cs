using Domain.Sample;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;

namespace Integration.Tests.Endpoints.Sample;

[TestFixture]
public sealed class SampleControllerEndpointTests : ApiEndpointsTestFixture
{
    private const string BaseUrl = "https://localhost:7065/";

    [Test]
    public async Task GetSample_ShouldReturnSample_WhenSampleExists()
    {
        // Arrange
        var sampleId = Guid.NewGuid();
        

        // Act
        var response = await Client.GetAsync($"{BaseUrl}/GetSample?id={sampleId}");

        // Assert
        var sample = await response.Content.ReadFromJsonAsync<SampleModel>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(sample, Is.Not.Null);
        Assert.That(sample.Id, Is.EqualTo(sampleId));
    }

    [Test]
    public async Task GetAllSamples_ShouldReturnAllSamples()
    {
        // Arrange

        // Act
        var response = await Client.GetAsync($"{BaseUrl}/GetAllSample");

        // Assert
        var samples = await response.Content.ReadFromJsonAsync<ImmutableHashSet<SampleDto>>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(samples, Is.Not.Empty);
    }

    [Test]
    public async Task CreateSample_ShouldCreateSample_WhenModelIsValid()
    {
        // Arrange
        var sample = new SampleDto(Guid.NewGuid(), "Sample Name", "Description", 100, DateTime.Now, DateTime.Now);

        // Act
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/CreateSample", sample);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteSample_ShouldReturnOk_WhenSampleExists()
    {
        // Arrange
        var sampleId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"{BaseUrl}/DeleteSample?id={sampleId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteSample_ShouldReturnBadRequest_WhenSampleDoesNotExist()
    {
        // Arrange
        var nonExistentSampleId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"{BaseUrl}/DeleteSample?id={nonExistentSampleId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}