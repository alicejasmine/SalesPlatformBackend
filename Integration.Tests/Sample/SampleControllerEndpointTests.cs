using Domain.Sample;
using Integration.Tests.Library;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;
using Api.Service.Usage;
using Test.Fixtures.Sample;

namespace Integration.Tests.Sample;

[TestFixture]
[TestOf(typeof(GetMonthlyUsageEndpoint))]
internal sealed class SampleControllerEndpointTests : BaseEndpointTests
{
    private const string BaseUrl = "https://localhost:7065";

    [Test]
    public async Task GetSample_ShouldReturnSample_WhenSampleExists()
    {
        // Arrange
        await Data.StoreUser(SampleModelFixture.DefaultSample);

        // Act
        var response = await AppHttpClient.GetAsync($"{BaseUrl}/GetSample?id={SampleModelFixture.DefaultSample.Id}");

        // Assert
        var sample = await response.Content.ReadFromJsonAsync<SampleModel>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(sample, Is.Not.Null);
        Assert.That(sample.Id, Is.EqualTo(SampleModelFixture.DefaultSample.Id));
    }

    [Test]
    public async Task GetAllSamples_ShouldReturnAllSamples()
    {
        // Arrange
        await Data.StoreUser(SampleModelFixture.DefaultSample);

        // Act
        var response = await AppHttpClient.GetAsync($"{BaseUrl}/GetAllSamples");

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
        var response = await AppHttpClient.PostAsJsonAsync($"{BaseUrl}/CreateSample", sample);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

    }

    [Test]
    public async Task DeleteSample_ShouldReturnOk_WhenSampleExists()
    {
        // Arrange
        await Data.StoreUser(SampleModelFixture.DefaultSample);

        // Act
        var response = await AppHttpClient.DeleteAsync($"{BaseUrl}/DeleteSample?id={SampleModelFixture.DefaultSample.Id}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteSample_ShouldReturnNotFound_WhenSampleDoesNotExist()
    {
        // Arrange
        var nonExistentSampleId = Guid.NewGuid();

        // Act
        var response = await AppHttpClient.DeleteAsync($"{BaseUrl}/DeleteSample?id={nonExistentSampleId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}