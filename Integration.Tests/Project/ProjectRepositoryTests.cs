using Infrastructure.Repositories.Project;
using Integration.Tests.Library;
using TestFixtures.Project;

namespace Integration.Tests.Project;

internal sealed class ProjectRepositoryTests : BaseRepositoryTests<ProjectRepository>
{
    protected override ProjectRepository CreateRepository() => new(DbContext);

    [Test]
    public async Task GetProjectByAlias_ReturnsModel_WhenFound()
    {
        //Arrange
        await Repository.UpsertAsync(ProjectModelFixture.DefaultProject);

        //Act
        var project = await Repository.GetProjectByAlias(ProjectEntityFixture.DefaultProject.Alias);

        //Assert
        Assert.That(project, Is.Not.Null);
        Assert.That(project.Alias, Is.EqualTo(ProjectEntityFixture.DefaultProject.Alias));
        Assert.That(project.DisplayName, Is.EqualTo(ProjectEntityFixture.DefaultProject.DisplayName));
    }

    [Test]
    public async Task GetProjectByAlias_ReturnsNull_WhenNotFound()
    {
        //Arrange
        await Repository.UpsertAsync(ProjectModelFixture.DefaultProject);

        //Act
        var project = await Repository.GetProjectByAlias("non-existent-alias");

        //Assert
        Assert.That(project, Is.Null);
    }
}