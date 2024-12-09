using Infrastructure;
using Infrastructure.Repositories.Organization;
using Infrastructure.Repositories.Project;
using Integration.Tests.Library;
using Microsoft.EntityFrameworkCore;
using TestFixtures.Organization;
using TestFixtures.Project;

namespace Integration.Tests.Project;

[TestFixture]
internal sealed class ProjectRepositoryTests :BaseDatabaseTestFixture
{
    private ProjectRepository _projectRepository;
    private OrganizationRepository _organizationRepository;
  

    [SetUp]
    public async Task SetUp()
    {
        _projectRepository = new(DatabaseTestsFixture.DbContext);
        _organizationRepository = new (DatabaseTestsFixture.DbContext);
    }
    
    [Test]
    public async Task GetProjectByAlias_ReturnsModel_WhenFound()
    {
        //Arrange
        await _organizationRepository.UpsertAsync(OrganizationModelFixture.DefaultOrganization);
        var expectedProject = ProjectModelFixture.DefaultProject;
        await _projectRepository.UpsertAsync(ProjectModelFixture.DefaultProject);

        //Act
        var project = await _projectRepository.GetProjectByAlias(expectedProject.Alias);

        //Assert
        Assert.That(project, Is.Not.Null);
        Assert.That(project.Alias, Is.EqualTo(expectedProject.Alias));
        Assert.That(project.DisplayName, Is.EqualTo(expectedProject.DisplayName));
    }

    [Test]
    public async Task GetProjectByAlias_ReturnsNull_WhenNotFound()
    {
        //Arrange & Act
        var project = await _projectRepository.GetProjectByAlias("non-existent-alias");

        //Assert
        Assert.That(project, Is.Null);
    }
}