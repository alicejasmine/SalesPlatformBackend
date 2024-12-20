using ApplicationServices.Sample;
using Domain.Sample;
using Integration.Tests.Library;
using Moq;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;
using Test.Fixtures.Sample;

namespace Integration.Tests.Sample;

[TestFixture]
internal sealed class SampleEndpointsTests : BaseEndpointTests
{
    [Test]
    public async Task GetSample_ShouldReturnSample_WhenSampleExists()
    {
        // Arrange
        await Data.StoreSample(SampleModelFixture.DefaultSample);

        // Act
        var response = await AppHttpClient.GetAsync($"/GetSample?id={SampleModelFixture.DefaultSample.Id}");

        // Assert
        var sample = await response.Content.ReadFromJsonAsync<SampleModel>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(sample, Is.Not.Null);
        Assert.That(sample.Id, Is.EqualTo(SampleModelFixture.DefaultSample.Id));
    }

    [Test]
    public async Task GetSample_ShouldReturnProblem404_WhenNoSampleExist()
    {
        //Arrange
        var nonExistingGuid = Guid.NewGuid();

        // Act
        var response = await AppHttpClient.GetAsync($"/GetSample?id={nonExistingGuid}");

        // Assert
        var sample = await response.Content.ReadFromJsonAsync<SampleModel>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(sample, Is.Not.Null);
        Assert.That(sample.Id, Is.Not.EqualTo(nonExistingGuid));
        Assert.That(sample.Id, Is.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task GetAllSamples_ShouldReturnAllSamples()
    {
        // Arrange
        await Data.StoreSample(SampleModelFixture.DefaultSample);

        // Act
        var response = await AppHttpClient.GetAsync($"/GetAllSamples");

        // Assert
        var samples = await response.Content.ReadFromJsonAsync<ImmutableHashSet<SampleDto>>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(samples, Is.Not.Empty);
    }

    [Test]
    public async Task GetAllSamples_ShouldReturnEmpty_WhenNoData()
    {
        // Act
        var response = await AppHttpClient.GetAsync($"/GetAllSamples");

        // Assert
        var samples = await response.Content.ReadFromJsonAsync<ImmutableHashSet<SampleDto>>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(samples, Is.Empty);
    }

    [Test]
    public async Task CreateSample_ShouldCreateSample_WhenModelIsValid()
    {
        // Arrange
        var sample = new SampleDto(Guid.NewGuid(), "Sample Name", "Description", 100, DateTime.Now, DateTime.Now);

        // Act
        var response = await AppHttpClient.PostAsJsonAsync($"/CreateSample", sample);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var storedSample = await DatabaseTestsFixture.DbContext.SampleEntities.FindAsync(sample.Id);
        Assert.That(storedSample, Is.Not.Null);
        Assert.That(storedSample!.Name, Is.EqualTo(sample.Name));
        Assert.That(storedSample.Description, Is.EqualTo(sample.Description));
    }

    [Test]
    public async Task CreateSample_ShouldReturnBadRequest_WhenModelIsInvalid()
    {
        // Arrange
        var invalidSample = new SampleDto(
            Guid.Empty,
            "",
            null!,
            -1,
            DateTime.UtcNow,
            DateTime.UtcNow);

        // Act
        var response = await AppHttpClient.PostAsJsonAsync("/CreateSample", invalidSample);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateSample_ShouldReturnBadRequest_WhenDtoIsNull()
    {
        // Act
        var response = await AppHttpClient.PostAsJsonAsync("/CreateSample", null as SampleDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task DeleteSample_ShouldReturnOk_WhenSampleExists()
    {
        // Arrange
        await Data.StoreSample(SampleModelFixture.DefaultSample);

        // Act
        var response = await AppHttpClient.DeleteAsync($"/DeleteSample?id={SampleModelFixture.DefaultSample.Id}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteSample_ShouldReturnNotFound_WhenSampleDoesNotExist()
    {
        // Arrange
        var nonExistentSampleId = Guid.NewGuid();

        // Act
        var response = await AppHttpClient.DeleteAsync($"DeleteSample?id={nonExistentSampleId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateSample_ShouldReturnOk_WhenSampleExistsAndIsValid()
    {
        // Arrange
        await Data.StoreSample(SampleModelFixture.DefaultSample);
        var updatedSample = SampleModelFixture.DefaultSample;
        updatedSample.Name = "Updated Name";
        updatedSample.Description = "Updated Description";

        // Act
        var response = await AppHttpClient.PutAsJsonAsync("/UpdateSample", updatedSample);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var storedSample = await DatabaseTestsFixture.DbContext.SampleEntities.FindAsync(updatedSample.Id);
        Assert.That(storedSample, Is.Not.Null);
        Assert.That(storedSample.Name, Is.EqualTo("Updated Name"));
        Assert.That(storedSample.Description, Is.EqualTo("Updated Description"));
    }

    [Test]
    public async Task UpdateSample_ShouldReturnNotFound_WhenSampleDoesNotExist()
    {
        // Arrange
        var nonExistentSample = new SampleDto(
            Guid.NewGuid(),
            "Non-Existent Sample",
            "Does not exist",
            100,
            DateTime.UtcNow,
            DateTime.UtcNow);

        // Act
        var response = await AppHttpClient.PutAsJsonAsync("/UpdateSample", nonExistentSample);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateSample_ShouldReturnBadRequest_WhenModelIsInvalid()
    {
        // Arrange
        var invalidSample = new SampleDto(
            Guid.Empty,  
            "",          
            null!,       
            -1,          
            DateTime.UtcNow,
            DateTime.UtcNow);

        // Act
        var response = await AppHttpClient.PutAsJsonAsync("/UpdateSample", invalidSample);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}