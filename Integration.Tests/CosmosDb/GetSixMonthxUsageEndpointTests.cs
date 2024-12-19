using Api.Service.Usage;
using Api.Service.Usage.DTOs;
using Domain.Entities;
using Integration.Tests.Library.Cosmo;
using System.Net;
using System.Net.Http.Json;
using Test.Fixtures.Usage;
using TestFixtures.Organization;
using TestFixtures.Project;
using TestFixtures.Usage;

namespace Integration.Tests.CosmosDb;

[TestFixture]
[TestOf(typeof(GetSixMonthsUsageEndpoint))]
internal class GetSixMonthxUsageEndpointTests : BaseCosmoEndpointTests
{
    [Test]
    public async Task GetSixMonthsUsage_ReturnsData_WhenDataExists()
    {
        //Arrange 
        var organization = OrganizationModelFixture.DefaultOrganization;
        var usage = UsageEntityFixture.DefaultUsage;
        var project = ProjectModelFixture.DefaultProject;
        var baseDate = usage.DocumentCreationDate;
        var expectedBandwidth = usage.TotalMonthlyBandwidth * 6;
        var expectedMedia = usage.TotalMonthlyMedia * 6;

        await Data.StoreOrganization(organization);
        await Data.StoreProject(project);

        var usageEntities = new List<UsageEntity>
        {
            new UsageEntityBuilder().WithEnvironmentId(project.EnvironmentId, baseDate).WithDate(baseDate).Build(),
            new UsageEntityBuilder().WithEnvironmentId(project.EnvironmentId, baseDate.AddMonths(-1)).WithDate(baseDate.AddMonths(-1)).Build(),
            new UsageEntityBuilder().WithEnvironmentId(project.EnvironmentId, baseDate.AddMonths(-2)).WithDate(baseDate.AddMonths(-2)).Build(),
            new UsageEntityBuilder().WithEnvironmentId(project.EnvironmentId, baseDate.AddMonths(-3)).WithDate(baseDate.AddMonths(-3)).Build(),
            new UsageEntityBuilder().WithEnvironmentId(project.EnvironmentId, baseDate.AddMonths(-4)).WithDate(baseDate.AddMonths(-4)).Build(),
            new UsageEntityBuilder().WithEnvironmentId(project.EnvironmentId, baseDate.AddMonths(-5)).WithDate(baseDate.AddMonths(-5)).Build(),
        };
        foreach (var entity in usageEntities)
        {
            await Data.StoreUsage(entity);
        }

        //Act
        var response = await AppHttpClient.GetAsync($"Usage/GetSixMonthsUsage?alias={project.Alias}&year={usage.DocumentCreationDate.Year}&month={usage.DocumentCreationDate.Month}");

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseUsage = await response.Content.ReadFromJsonAsync<TotalUsageResponse>();
        Assert.That(responseUsage, Is.Not.Null);
        Assert.That(responseUsage.TotalDurationMedia.Count, Is.EqualTo(6));
        Assert.That(responseUsage.TotalDurationBandwidth.Count, Is.EqualTo(6));
        var firstBandwidth = responseUsage.TotalDurationBandwidth[0];
        Assert.That(firstBandwidth, Is.EqualTo(usage.TotalMonthlyBandwidth));
        var firstmedia = responseUsage.TotalDurationMedia[0];
        Assert.That(firstmedia, Is.EqualTo(usage.TotalMonthlyMedia));
    }

    [Test]
    public async Task GetSixMonthsUsage_ReturnsNull_WhenNoDataExists()
    {
        //Arrange 
        var nonExistingAlias = "non-existing-alias";

        //Act
        var response = await AppHttpClient.GetAsync($"Usage/GetMonthlyUsage?alias={nonExistingAlias}&year={2024}&month={1}");

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}