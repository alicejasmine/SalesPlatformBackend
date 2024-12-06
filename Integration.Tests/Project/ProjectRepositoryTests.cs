using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories.Project;
using TestFixtures.Organization;
using TestFixtures.Project;

namespace Integration.Tests.Project
{
    [TestFixture]
    public class ProjectRepositoryTests
    {
        private SalesPlatformDbContext _dbContext;
        private ProjectRepository _projectRepository;

        [SetUp]
        public void Setup()
        {
            var connectionString =
                "Server=localhost,1433;Database=SalesPlatformDB;User Id=sa;Password=Suits0811;TrustServerCertificate=True;";

            var options = new DbContextOptionsBuilder<SalesPlatformDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            _dbContext = new SalesPlatformDbContext(options);

            _projectRepository = new ProjectRepository(_dbContext);

            _dbContext.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetEnvironmentIdByAlias_ReturnsEnvironmentId_WhenAliasExists()
        {
            // Arrange
            var alias = "oxygen-website1";
            var projectEntity = ProjectEntityFixture.DefaultProject;
            var expectedEnvironmentId = projectEntity.EnvironmentId;

            var organizationEntity = OrganizationEntityFixture.DefaultOrganization;
            await _dbContext.organizationEntities.AddAsync(organizationEntity);
            await _dbContext.SaveChangesAsync(); //Ensure the OrganizationEntity is saved before adding ProjectEntity
            projectEntity.OrganizationId = organizationEntity.Id;

            await _dbContext.ProjectEntities.AddAsync(projectEntity);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _projectRepository.GetEnvironmentIdByAlias(alias);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEnvironmentId));
        }


        [Test]
        public async Task GetEnvironmentIdByAlias_ThrowsKeyNotFoundException_WhenAliasDoesNotExist()
        {
            // Arrange
            var alias = "non-existent-alias";

            //Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _projectRepository.GetEnvironmentIdByAlias(alias)
            );

            Assert.That(ex.Message, Is.EqualTo($"No project found with alias '{alias}'"));
        }

        [Test]
        public void GetEnvironmentIdByAlias_ThrowsArgumentException_WhenAliasIsEmpty()
        {
            // Arrange
            var alias = "";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(
                async () => await _projectRepository.GetEnvironmentIdByAlias(alias)
            );

            Assert.That(ex.ParamName, Is.EqualTo("alias"));
            Assert.That(ex.Message, Does.Contain("Alias cannot be null or empty"));
        }
    }
}
