using ApplicationServices.Project;
using Domain.Models;
using Infrastructure.Repositories.Project;
using Moq;
using TestFixtures.Project;

namespace Tests.Services
{
    [TestFixture]
    public class ProjectServiceTests
    {
        private Mock<IProjectRepository> _mockProjectRepository;
        private ProjectService _projectService;

        [SetUp]
        public void SetUp()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _projectService = new ProjectService(_mockProjectRepository.Object);
        }

        [Test]
        public async Task GetProjectByAlias_ReturnsProject_WhenFound()
        {
            //Arrange
            var expectedProject = ProjectModelFixture.DefaultProject;
            
            _mockProjectRepository.Setup(repo => repo.GetProjectByAlias(expectedProject.Alias))
                .ReturnsAsync(expectedProject);

            //Act
            var result = await _projectService.GetProjectByAlias(expectedProject.Alias);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Alias, Is.EqualTo(expectedProject.Alias));
            Assert.That(result?.DisplayName, Is.EqualTo(expectedProject.DisplayName));
        }

        [Test]
        public async Task GetProjectByAlias_ReturnsNull_WhenNotFound()
        {
            //Arrange
            var alias = "non-existent-alias";
            
            _mockProjectRepository.Setup(repo => repo.GetProjectByAlias(alias))
                .ReturnsAsync((ProjectModel?)null);

            //Act
            var result = await _projectService.GetProjectByAlias(alias);

            //Assert
            Assert.That(result, Is.Null);
        }
    }
}
