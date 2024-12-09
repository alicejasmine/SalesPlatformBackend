using Infrastructure.Repositories.Organization;
using Infrastructure.Repositories.Project;
using Integration.Tests.Library;
using TestFixtures.Organization;
using TestFixtures.Project;

namespace Integration.Tests.Project;

[TestFixture]
internal sealed class ProjectRepositoryTests : BaseRepositoryTests<ProjectRepository>
{
    
    private OrganizationRepository _organizationRepository;
    protected override ProjectRepository CreateRepository() => new(DbContext);
    
    [SetUp]
    public async Task SetUp()
    {
        _organizationRepository = new OrganizationRepository(DbContext);
    }
    
    [Test]
    public async Task GetProjectByAlias_ReturnsModel_WhenFound()
    {
        //Arrange
        await _organizationRepository.UpsertAsync(OrganizationModelFixture.DefaultOrganization);
        var expectedProject = ProjectModelFixture.DefaultProject;
        await Repository.UpsertAsync(ProjectModelFixture.DefaultProject);

        //Act
        var project = await Repository.GetProjectByAlias(expectedProject.Alias);

        //Assert
        Assert.That(project, Is.Not.Null);
        Assert.That(project.Alias, Is.EqualTo(expectedProject.Alias));
        Assert.That(project.DisplayName, Is.EqualTo(expectedProject.DisplayName));
    }

    [Test]
    public async Task GetProjectByAlias_ReturnsNull_WhenNotFound()
    {
        //Arrange & Act
        var project = await Repository.GetProjectByAlias("non-existent-alias");

        //Assert
        Assert.That(project, Is.Null);
    }
}