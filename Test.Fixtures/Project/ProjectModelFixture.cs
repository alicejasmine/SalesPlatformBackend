using Domain.Models;

namespace TestFixtures.Project;

public class ProjectModelFixture
{
    private static readonly Guid DefaultEnvironmentId = Guid.Parse("3b95c098-21b4-4f8b-9f7c-6a435ed83931");
    private static readonly Guid DefaultPlanId = Guid.Parse("d5476bbd-bd30-4f54-8a63-b1bc1a8e4bc2");
    private static readonly Guid DefaultOrganizationId = Guid.Parse("c1aaf1b9-5824-4e27-9b29-3d5ebc5d66d4");
    private static readonly DateTime DefaultCreated = DateTime.UtcNow;
    private static readonly DateTime DefaultModified = DateTime.UtcNow;
        
    private const string DefaultAlias = "oxygen-website4";
    private const string DefaultDisplayName = "Oxygen Website";
    public static ProjectModel DefaultProject { get; } = new ProjectModel(
        id: Guid.NewGuid(),
        environmentId:DefaultEnvironmentId,
        alias: DefaultAlias,
        displayName: DefaultDisplayName,
        planId: DefaultPlanId,
        organizationId: DefaultOrganizationId,
        created: DefaultCreated,
        modified: DefaultModified
    );
}