using Domain.Entities;
using Domain.Enum;
using Domain.Models;
using Infrastructure.Repositories.Credit;
using Infrastructure.Repositories.Organization;
using Infrastructure.Repositories.Plan;
using Infrastructure.Repositories.Project;
using Infrastructure.Repositories.Usage;

namespace ApplicationServices.Seed;

public class SeedService : ISeedService
{
    private readonly IUsageDocumentRepository _usageDocumentRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IPlanRepository _planRepository;
    private readonly ICreditRepository _creditRepository;

    private List<Guid> PlanIds = new List<Guid>();
    private List<Guid> OrganizationIds = new List<Guid>();
    private List<Guid> ProjectIds = new List<Guid>();
    private List<Guid> EnvironmentIds = new List<Guid>();

    public SeedService(
        IUsageDocumentRepository usageDocumentRepository,
        IOrganizationRepository organizationRepository,
        IProjectRepository projectRepository,
        IPlanRepository planRepository, 
        ICreditRepository creditRepository)
    {
        _usageDocumentRepository = usageDocumentRepository;
        _organizationRepository = organizationRepository;
        _projectRepository = projectRepository;
        _planRepository = planRepository;
        _creditRepository = creditRepository;
    }

    public async Task SeedDatabasesWithData()
    {
        await SeedPlans();
        await SeedOrganization();
        await SeedProjects();
        await SeedUsage();
        await SeedCredits();
    }

    private async Task SeedPlans()
    {
        var plans = new List<PlanModel>
        {
                new PlanModel(Guid.NewGuid(), "Starter Plan", 290, PlanEnum.Starter, DateTime.UtcNow, DateTime.UtcNow),
                new PlanModel(Guid.NewGuid(), "Standard Plan", 1800, PlanEnum.Standard, DateTime.UtcNow, DateTime.UtcNow),
                new PlanModel(Guid.NewGuid(), "Professional Plan", 4800, PlanEnum.Professional, DateTime.UtcNow, DateTime.UtcNow),
                new PlanModel(Guid.NewGuid(), "Enterprise Plan", 100000, PlanEnum.Enterprise, DateTime.UtcNow, DateTime.UtcNow)
        };

        foreach (var plan in plans)
        {
            await _planRepository.UpsertAsync(plan);
            PlanIds.Add(plan.Id);
        }
    }

    private async Task SeedOrganization()
    {
        var Organizations = new List<OrganizationModel>
        {
            new OrganizationModel(Guid.NewGuid(), "oxygen", "Oxygen", 154827, PartnershipEnum.Gold, DateTime.UtcNow, DateTime.UtcNow,  new List<CreditHistoryModel>()),
            new OrganizationModel(Guid.NewGuid(), "io", "iO", 789456, PartnershipEnum.Platinum, DateTime.UtcNow, DateTime.UtcNow,  new List<CreditHistoryModel>()),
            new OrganizationModel(Guid.NewGuid(), "increo", "Increo", 784, PartnershipEnum.Silver, DateTime.UtcNow, DateTime.UtcNow,  new List<CreditHistoryModel>()),
        };

        foreach (var organization in Organizations)
        {
            await _organizationRepository.UpsertAsync(organization);
            OrganizationIds.Add(organization.Id);
        }
        
    }

    private async Task SeedProjects()
    {
        var projects = new List<ProjectModel>
        {
            //oxygen    
            new ProjectModel(Guid.NewGuid(), Guid.NewGuid(), "oxygen-website1", "Lego", PlanIds[3], OrganizationIds[0], DateTime.UtcNow, DateTime.UtcNow),
            new ProjectModel(Guid.NewGuid(), Guid.NewGuid(), "oxygen-website2", "British Army", PlanIds[3], OrganizationIds[0], DateTime.UtcNow, DateTime.UtcNow),
            new ProjectModel(Guid.NewGuid(), Guid.NewGuid(), "oxygen-website3", "Local brewery", PlanIds[0], OrganizationIds[0], DateTime.UtcNow, DateTime.UtcNow),

            //io
            new ProjectModel(Guid.NewGuid(), Guid.NewGuid(), "io-website1", "Carlsberg", PlanIds[3], OrganizationIds[1], DateTime.UtcNow, DateTime.UtcNow),
            new ProjectModel(Guid.NewGuid(), Guid.NewGuid(), "io-website2", "Royal Unibrew", PlanIds[1], OrganizationIds[1], DateTime.UtcNow, DateTime.UtcNow),
            new ProjectModel(Guid.NewGuid(), Guid.NewGuid(), "io-website3", "Redbull", PlanIds[2], OrganizationIds[1], DateTime.UtcNow, DateTime.UtcNow),

            //increa
            new ProjectModel(Guid.NewGuid(), Guid.NewGuid(), "increa-website1", "National Museum", PlanIds[0], OrganizationIds[2], DateTime.UtcNow, DateTime.UtcNow),
            new ProjectModel(Guid.NewGuid(), Guid.NewGuid(), "increa-website2", "Local Museum", PlanIds[2], OrganizationIds[2], DateTime.UtcNow, DateTime.UtcNow),
            new ProjectModel(Guid.NewGuid(), Guid.NewGuid(), "increa-website3", "UN", PlanIds[3], OrganizationIds[2], DateTime.UtcNow, DateTime.UtcNow),
        };

        foreach (var project in projects)
        {
            await _projectRepository.UpsertAsync(project);
            ProjectIds.Add(project.Id);
            EnvironmentIds.Add(project.EnvironmentId);
        }
    }
    private async Task SeedCredits()
    {
        var creditHistories = new List<CreditHistoryModel>
        {
            new CreditHistoryModel(
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                351358,
                1000,
                200,
                800,
                new DateOnly(2024, 12, 17),
                new DateOnly(2024, 12, 18),
                OrganizationIds[0] 
            ),
            new CreditHistoryModel(
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                351359,
                2000,
                500,
                1500,
                new DateOnly(2024, 12, 19),
                new DateOnly(2024, 12, 20),
                OrganizationIds[1] 
            )
        };

        foreach (var creditHistory in creditHistories)
        {
            await _creditRepository.UpsertAsync(creditHistory);
        }
        
        foreach (var creditHistory in creditHistories)
        {
            var organization = await _organizationRepository.GetByIdAsync(creditHistory.OrganizationId);
            if (organization != null)
            {
                organization.CreditHistory.Add(creditHistory);
                await _organizationRepository.UpsertAsync(organization);
            }
        }
    }


    private async Task SeedUsage()
    {
        for (int i = 0; i < ProjectIds.Count; i++)
        {
            await _usageDocumentRepository.SeedUsageDocument(ProjectIds[i], EnvironmentIds[i]);

        }
    }
}