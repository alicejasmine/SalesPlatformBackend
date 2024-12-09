using System.Net;
using System.Net.Http.Json;
using Api.Service.Project;
using Domain.Models;
using Integration.Tests.Library;
using Microsoft.AspNetCore.Mvc;
using TestFixtures.Organization;
using TestFixtures.Project;

namespace Integration.Tests.Project;

[TestFixture]
[TestOf(typeof(GetProjectByProjectAliasEndpoint))]
internal sealed class GetProjectByProjectAliasEndpointTests : BaseEndpointTests
{
    
    [Test]
    public async Task GetProjectByProjectAlias_ReturnsNotFound_WithDetails_WhenProjectNotFound()
    {
        //Arrange 
        var projectAlias = "non existent alias";
        
        //Act
        using var response = await AppHttpClient.GetAsync($"/GetProjectByAlias?alias={projectAlias}");

        //Assert
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.That(problemDetails, Is.Not.Null);
        Assert.That(problemDetails!.Title, Is.EqualTo("Project not found"));
        Assert.That(problemDetails.Detail, Does.Contain(projectAlias));
    }

    [Test]
    public async Task GetProjectByProjectAlias_ReturnsProject_WhenSuccess()
    {
        //Arrange
        var project = ProjectModelFixture.DefaultProject;
        await Data.StoreOrganization(OrganizationModelFixture.DefaultOrganization);
        await Data.StoreProject(project);

        //Act
        using var response = await AppHttpClient.GetAsync($"/GetProjectByAlias?alias={project.Alias}");

        //Assert
        var projectResponse = await response.Content.ReadFromJsonAsync<ProjectModel>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(projectResponse, Is.Not.Null);
        Assert.That(projectResponse.Alias, Is.EqualTo(ProjectEntityFixture.DefaultProject.Alias));
        Assert.That(projectResponse.DisplayName, Is.EqualTo(ProjectEntityFixture.DefaultProject.DisplayName));
        Assert.That(projectResponse.EnvironmentId, Is.EqualTo(ProjectEntityFixture.DefaultProject.EnvironmentId));
        Assert.That(projectResponse.PlanId, Is.EqualTo(ProjectEntityFixture.DefaultProject.PlanId));
        Assert.That(projectResponse.OrganizationId, Is.EqualTo(ProjectEntityFixture.DefaultProject.OrganizationId));
        Assert.That(projectResponse.Created.Date, Is.EqualTo(ProjectEntityFixture.DefaultProject.Created.Date));
        Assert.That(projectResponse.Modified.Date, Is.EqualTo(ProjectEntityFixture.DefaultProject.Modified.Date));
    }
}