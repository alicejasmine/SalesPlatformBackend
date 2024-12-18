using ApplicationServices.Seed;
using Domain.Models;
using Infrastructure.Repositories.Credit;
using Infrastructure.Repositories.Organization;
using Infrastructure.Repositories.Plan;
using Infrastructure.Repositories.Project;
using Infrastructure.Repositories.Usage;
using Moq;
using NUnit.Framework;

namespace Tests.Services
{
    public class SeedServiceTests
    {
        private SeedService _seedService;
        private Mock<IUsageDocumentRepository> _usageDocumentRepository;
        private Mock<IOrganizationRepository> _organizationRepository;
        private Mock<IProjectRepository> _projectRepository;
        private Mock<IPlanRepository> _planRepository;
        private Mock<ICreditRepository> _creditRepository;

        [SetUp]
        public void Setup()
        {
            _usageDocumentRepository = new Mock<IUsageDocumentRepository>();
            _organizationRepository = new Mock<IOrganizationRepository>();
            _projectRepository = new Mock<IProjectRepository>();
            _planRepository = new Mock<IPlanRepository>();
            _creditRepository = new Mock<ICreditRepository>();

            _seedService = new SeedService(
                _usageDocumentRepository.Object,
                _organizationRepository.Object,
                _projectRepository.Object,
                _planRepository.Object,
                _creditRepository.Object
            );
        }

        [Test]
        public async Task SeedPlans_ShouldSeedAllPlans_WhenSuccess()
        {
            // Arrange
            var capturedPlans = new List<PlanModel>();

            _planRepository
                .Setup(repo => repo.UpsertAsync(It.IsAny<PlanModel>()))
                .Callback<PlanModel>(plan => capturedPlans.Add(plan))
                .ReturnsAsync((PlanModel plan) => plan);

            // Act
            await _seedService.SeedDatabasesWithData();

            // Assert
            Assert.That(capturedPlans.Count, Is.EqualTo(4));
            Assert.That(capturedPlans.Select(p => p.Name), Is.EquivalentTo(new[] { "Starter Plan", "Standard Plan", "Professional Plan", "Enterprise Plan" }));
        }

        [Test]
        public async Task SeedOrganizations_ShouldSeedAllOrganizations_WhenSuccess()
        {
            // Arrange
            var capturedOrganizations = new List<OrganizationModel>();

            _organizationRepository
                .Setup(repo => repo.UpsertAsync(It.IsAny<OrganizationModel>()))
                .Callback<OrganizationModel>(organization => capturedOrganizations.Add(organization))
                .ReturnsAsync((OrganizationModel organization) => organization);

            // Act
            await _seedService.SeedDatabasesWithData();

            // Assert
            Assert.That(capturedOrganizations.Count, Is.EqualTo(3));
            Assert.That(capturedOrganizations.Select(o => o.Alias), Is.EquivalentTo(new[] { "oxygen", "io", "increo" }));
        }

        [Test]
        public async Task SeedProjects_ShouldSeedAllProjects_WhenSuccess()
        {
            // Arrange
            var capturedProjects = new List<ProjectModel>();

            _projectRepository
                .Setup(repo => repo.UpsertAsync(It.IsAny<ProjectModel>()))
                .Callback<ProjectModel>(project => capturedProjects.Add(project))
                .ReturnsAsync((ProjectModel project) => project);

            // Act
            await _seedService.SeedDatabasesWithData();

            // Assert
            Assert.That(capturedProjects.Count, Is.EqualTo(9));
            Assert.That(capturedProjects.Select(p => p.DisplayName), Does.Contain("Lego"));
            Assert.That(capturedProjects.Select(p => p.DisplayName), Does.Contain("British Army"));
            Assert.That(capturedProjects.Select(p => p.DisplayName), Does.Contain("UN"));
        }

        [Test]
        public async Task SeedUsage_ShouldCallRepositoryCorrectNumberOfTimes_WhenSuccess()
        {
            // Arrange
            var capturedProjectIds = new List<Guid>();
            var capturedEnvironmentIds = new List<Guid>();

            _usageDocumentRepository
                .Setup(repo => repo.SeedUsageDocument(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Callback<Guid, Guid>((projectId, environmentId) =>
                {
                    capturedProjectIds.Add(projectId);
                    capturedEnvironmentIds.Add(environmentId);
                })
                .Returns(Task.CompletedTask);

            // Act
            await _seedService.SeedDatabasesWithData();

            // Assert
            Assert.That(capturedProjectIds.Count, Is.EqualTo(9));
            Assert.That(capturedEnvironmentIds.Count, Is.EqualTo(9));
            Assert.That(capturedProjectIds, Is.Unique);
            Assert.That(capturedEnvironmentIds, Is.Unique);
        }

        [Test]
        public async Task SeedUsage_ShouldGenerateValidData_WhenSuccess()
        {
            // Arrange
            var capturedProjectIds = new List<Guid>();
            var capturedEnvironmentIds = new List<Guid>();

            _usageDocumentRepository
                .Setup(repo => repo.SeedUsageDocument(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Callback<Guid, Guid>((projectId, environmentId) =>
                {
                    capturedProjectIds.Add(projectId);
                    capturedEnvironmentIds.Add(environmentId);
                })
                .Returns(Task.CompletedTask);

            // Act
            await _seedService.SeedDatabasesWithData();

            // Assert
            Assert.That(capturedProjectIds, Has.All.Not.EqualTo(Guid.Empty));
            Assert.That(capturedEnvironmentIds, Has.All.Not.EqualTo(Guid.Empty));
        }
    }
}
