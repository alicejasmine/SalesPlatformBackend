using Api.Service.DTOs;
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
[TestOf(typeof(GetYearWorthOfTotalDataEndpoint))]
internal class GetYearWorthOfTotalDataEndpointTests : BaseCosmoEndpointTests
{
    [Test]
    public async Task GetMonthlyUsage_ReturnsData_WhenDataExists()
    {
        //Arrange 
        var organization = OrganizationModelFixture.DefaultOrganization;
        var usage = UsageEntityFixture.DefaultUsage;
        var project = ProjectModelFixture.DefaultProject;
        var baseDate = usage.DocumentCreationDate;
        var usageEntities = new List<UsageEntity>();
        var expectedBandwidth = usage.TotalMonthlyBandwidth * 12;
        var expectedMedia = usage.TotalMonthlyMedia * 12;

        await Data.StoreOrganization(organization);
        await Data.StoreProject(project);

        for (var i = 0; i < 12; i++)
        {
            usageEntities.Add(new UsageEntityBuilder().WithEnvironmentId(project.EnvironmentId, baseDate.AddMonths(-i)).WithDate(baseDate.AddMonths(-i)).Build());
        }
        foreach (var entity in usageEntities)
        {
            await Data.StoreUsage(entity);
        }

        //Act
        var response = await AppHttpClient.GetAsync($"Usage/GetYearWorthOfTotalData?alias={project.Alias}&year={usage.DocumentCreationDate.Year}&month={usage.DocumentCreationDate.Month}");

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseUsage = await response.Content.ReadFromJsonAsync<YearTotalUsageResponse>();
        Assert.That(responseUsage, Is.Not.Null);
        Assert.That(responseUsage.TotalMedia, Is.EqualTo(expectedMedia));
        Assert.That(responseUsage.TotalBandwidth, Is.EqualTo(expectedBandwidth));
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