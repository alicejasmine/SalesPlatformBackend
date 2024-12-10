using System.Net;
using System.Net.Http.Json;
using Api.Service.Project;
using Api.Service.Project.DTOs;
using Integration.Tests.Library;
using Microsoft.AspNetCore.Mvc;
using TestFixtures.Organization;
using TestFixtures.Project;

namespace Integration.Tests.Project;

[TestFixture]
[TestOf(typeof(GetProjectsByOrganizationAliasEndpoint))]
internal sealed class GetProjectsByOrganizationAliasEndpointTests : BaseEndpointTests
{
    
    [Test]
    public async Task GetProjectsByOrganizationAlias_ReturnsNotFound_WithDetails_WhenNotFound()
    {
        //Arrange 
        var organizationAlias = "nonexistentalias";
        
        //Act
        using var response = await AppHttpClient.GetAsync($"/GetProjectsByOrganizationAlias?organizationalias={organizationAlias}");
        
        //Assert
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(problemDetails, Is.Not.Null);
        Assert.That(problemDetails!.Title, Is.EqualTo("Projects not found"));
    }
    
    [Test]
    public async Task GetAllProjects_ReturnsProjectsList_WhenSuccess()
    {
        //Arrange
        var organization = OrganizationModelFixture.DefaultOrganization;
        var organizationAlias = organization.Alias;
        var project1 = ProjectModelFixture.DefaultProject;
        var project2 = ProjectModelFixture.OtherDefaultProject;
        await Data.StoreOrganization(organization);

        await Data.StoreProject(project1);
        await Data.StoreProject(project2);

        // Act
        using var response = await AppHttpClient.GetAsync($"/GetProjectsByOrganizationAlias?organizationalias={organizationAlias}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var projectResponse = await response.Content.ReadFromJsonAsync<List<ProjectResponse>>();
        Assert.That(projectResponse, Is.Not.Null);
        Assert.That(projectResponse.Count, Is.EqualTo(2)); 
     
        foreach (var actualProject in projectResponse)
        {
            Assert.That(actualProject.Alias, Is.EqualTo(ProjectModelFixture.DefaultProject.Alias));
            Assert.That(actualProject.DisplayName, Is.EqualTo(ProjectModelFixture.DefaultProject.DisplayName));
            Assert.That(actualProject.EnvironmentId, Is.EqualTo(ProjectModelFixture.DefaultProject.EnvironmentId));
            Assert.That(actualProject.PlanId, Is.EqualTo(ProjectModelFixture.DefaultProject.PlanId));
            Assert.That(actualProject.OrganizationId, Is.EqualTo(ProjectModelFixture.DefaultProject.OrganizationId));
            Assert.That(actualProject.Created.Date, Is.EqualTo(ProjectModelFixture.DefaultProject.Created.Date));
            Assert.That(actualProject.Modified.Date, Is.EqualTo(ProjectModelFixture.DefaultProject.Modified.Date));
        }
    }
}