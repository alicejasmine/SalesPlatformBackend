using Api.Service.DTOs;
using Api.Service.Usage;
using Integration.Tests.Library.Cosmo;
using System.Net;
using System.Net.Http.Json;
using Test.Fixtures.Usage;
using TestFixtures.Organization;
using TestFixtures.Project;

namespace Integration.Tests.CosmosDb;

[TestFixture]
[TestOf(typeof(GetMonthlyUsageEndpoint))]
internal class GetMonthlyUsageEndpointTests : BaseCosmoEndpointTests
{
    [Test]
    public async Task GetMonthlyUsage_ReturnsData_WhenDataExists()
    {
        //Arrange 
        var organization = OrganizationModelFixture.DefaultOrganization;
        var usage = UsageEntityFixture.DefaultUsage;
        var project = ProjectModelFixture.DefaultProject;

        await Data.StoreOrganization(organization);
        await Data.StoreProject(project);
        await Data.StoreUsage(usage);

        //Act
        var response = await AppHttpClient.GetAsync($"Usage/GetMonthlyUsage?alias={project.Alias}&year={usage.DocumentCreationDate.Year}&month={usage.DocumentCreationDate.Month}");

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseUsage = await response.Content.ReadFromJsonAsync<UsageResponse>();
        Assert.That(responseUsage, Is.Not.Null);
        Assert.That(responseUsage.EnvironmentId, Is.EqualTo(usage.EnvironmentId));
        Assert.That(responseUsage.TotalMonthlyBandwidth, Is.EqualTo(usage.TotalMonthlyBandwidth));
        Assert.That(responseUsage.TotalMonthlyMedia, Is.EqualTo(usage.TotalMonthlyMedia));
        Assert.That(responseUsage.DailyUsages.Count, Is.EqualTo(usage.Days.Count));
        foreach (var day in responseUsage.DailyUsages)
        {
            var usageDay = usage.Days[day.Date];
            Assert.That(day.MediaSizeInBytes, Is.EqualTo(usageDay.MediaSizeInBytes), $"Mismatch in MediaSizeInBytes for day: {day.Date}");
            Assert.That(day.BandwidthInBytes, Is.EqualTo(usageDay.BandwidthInBytes), $"Mismatch in BandwidthInBytes for day: {day.Date}");
            Assert.That(day.Hostnames, Is.EqualTo(usageDay.Hostnames), $"Mismatch in Hostnames for day: {day.Date}");
            Assert.That(day.ContentNodes, Is.EqualTo(usageDay.ContentNodes), $"Mismatch in ContentNodes for day: {day.Date}");
        }
    }

    [Test]
    public async Task GetMonthlyUsage_ReturnsNull_WhenNoDataExists()
    {
        //Arrange 
        var nonExistingAlias = "non-existing-alias";

        try
        {
            //Act
            var response = await AppHttpClient.GetAsync($"Usage/GetMonthlyUsage?alias={nonExistingAlias}&year={2024}&month={1}");
        }
        catch (Exception ex)
        {
            //Assert
            Assert.That(ex.Message, Is.EqualTo($"No project found with alias '{nonExistingAlias}'"));
        }
    }
}