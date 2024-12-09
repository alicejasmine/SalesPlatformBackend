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

        [Test]
        public async Task GetAllProjects_ReturnsProjects_WhenFound()
        {
            // Arrange
            var project1 = ProjectModelFixture.DefaultProject;
            var project2 = ProjectModelFixture.OtherDefaultProject;

            var expectedProjects = new List<ProjectModel>
            {
                project1, project2
            };

            _mockProjectRepository.Setup(repo => repo.GetAllProjects())
                .ReturnsAsync(expectedProjects);

            // Act
            var result = await _projectService.GetAllProjects();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Alias, Is.EqualTo(project1.Alias));
            Assert.That(result[1].Alias, Is.EqualTo(project2.Alias));
        }

        [Test]
        public async Task GetAllProjects_ReturnsEmptyList_WhenNoProjectsFound()
        {
            // Arrange
            _mockProjectRepository.Setup(repo => repo.GetAllProjects())
                .ReturnsAsync(new List<ProjectModel>());

            // Act
            var result = await _projectService.GetAllProjects();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
