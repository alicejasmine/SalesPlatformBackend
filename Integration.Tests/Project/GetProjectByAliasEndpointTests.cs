using System.Net;
using Api.Service.Project;
using Api.Service.Project.DTOs;
using Integration.Tests.Library;
using Integration.Tests.Library.Http;
using Microsoft.AspNetCore.Mvc;
using TestFixtures.Organization;
using TestFixtures.Project;

namespace Integration.Tests.Project;

[TestFixture]
[TestOf(typeof(GetProjectByAliasEndpoint))]
internal sealed class GetProjectByAliasEndpointTests : BaseEndpointTests
{
    private const string BaseUrl = "https://localhost:7065";
    
    [Test]
    public async Task GetProjectByAlias_ReturnsNotFound_WithDetails_WhenProjectNotFound()
    {
        //Arrange 
        var projectAlias = "non existent alias";
        
        //Act
        using var response = await AppHttpClient.GetAsync($"{BaseUrl}/GetProjectByAlias?alias={projectAlias}");

        //Assert
        var responseObject = HttpAssert.CanDeserializeResponseAs<ProblemDetails>(response);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(responseObject, Is.Not.Null);
        Assert.That(responseObject.Title, Is.EqualTo("Project not found"));
        Assert.That(responseObject.Detail, Does.Contain(projectAlias));
    }

    [Test]
    public async Task GetProjectByAlias_ReturnsProject_WhenSuccess()
    {
        //Arrange
        var project = ProjectModelFixture.DefaultProject;
        await Data.StoreOrganization(OrganizationModelFixture.DefaultOrganization);
        await Data.StoreProject(project);

        //Act
        using var response = await AppHttpClient.GetAsync($"{BaseUrl}/GetProjectByAlias?alias={project.Alias}");

        //Assert
        var responseObject = HttpAssert.CanDeserializeResponseAs<ProjectResponse>(response);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(responseObject, Is.Not.Null);
        Assert.That(responseObject.Alias, Is.EqualTo(ProjectEntityFixture.DefaultProject.Alias));
        Assert.That(responseObject.DisplayName, Is.EqualTo(ProjectEntityFixture.DefaultProject.DisplayName));
        Assert.That(responseObject.EnvironmentId, Is.EqualTo(ProjectEntityFixture.DefaultProject.EnvironmentId));
        Assert.That(responseObject.PlanId, Is.EqualTo(ProjectEntityFixture.DefaultProject.PlanId));
        Assert.That(responseObject.OrganizationId, Is.EqualTo(ProjectEntityFixture.DefaultProject.OrganizationId));
        Assert.That(responseObject.Created.Date, Is.EqualTo(ProjectEntityFixture.DefaultProject.Created.Date));
        Assert.That(responseObject.Modified.Date, Is.EqualTo(ProjectEntityFixture.DefaultProject.Modified.Date));
    }
}