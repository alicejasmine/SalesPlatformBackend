using Domain.Models;
using Infrastructure.Repositories.Organization;
using Infrastructure.Repositories.Project;
using Integration.Tests.Library;
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
    public async Task GetProjectByProjectAlias_ReturnsModel_WhenFound()
    {
        //Arrange
        await _organizationRepository.UpsertAsync(OrganizationModelFixture.DefaultOrganization);
        var expectedProject = ProjectModelFixture.DefaultProject;
        await _projectRepository.UpsertAsync(ProjectModelFixture.DefaultProject);

        //Act
        var project = await _projectRepository.GetProjectByProjectAlias(expectedProject.Alias);

        //Assert
        Assert.That(project, Is.Not.Null);
        Assert.That(project.Alias, Is.EqualTo(expectedProject.Alias));
        Assert.That(project.DisplayName, Is.EqualTo(expectedProject.DisplayName));
    }

    [Test]
    public async Task GetProjectByProjectAlias_ReturnsNull_WhenNotFound()
    {
        //Arrange & Act
        var project = await _projectRepository.GetProjectByProjectAlias("non-existent-alias");

        //Assert
        Assert.That(project, Is.Null);
    }
    
    [Test]
    public async Task GetAllProjects_ReturnsProjectList_WhenFound()
    {
        //Arrange
        await _organizationRepository.UpsertAsync(OrganizationModelFixture.DefaultOrganization);
        await _projectRepository.UpsertAsync(ProjectModelFixture.DefaultProject);
        await _projectRepository.UpsertAsync(ProjectModelFixture.OtherDefaultProject);

        //Act
        var projects = await _projectRepository.GetAllProjects();

        //Assert
        Assert.That(projects, Is.Not.Null);
        Assert.That(projects.Count, Is.EqualTo(2));
    }
    
    
    [Test]
    public async Task GetAllProjects_ReturnsEmptyList_WhenNoProjectsFound()
    {
        //Arrange & Act
        var projects = await _projectRepository.GetAllProjects();

        //Assert
        Assert.That(projects, Is.Empty);
    }
}