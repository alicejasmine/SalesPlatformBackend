using Domain.Entities;
using TestFixtures.Organization;

namespace TestFixtures.Project
{
    public static class ProjectEntityFixture
    {
        private static readonly Guid DefaultEnvironmentId = Guid.Parse("3b95c098-21b4-4f8b-9f7c-6a435ed83931");
        private static readonly Guid DefaultPlanId = Guid.Parse("d5476bbd-bd30-4f54-8a63-b1bc1a8e4bc2");
        private static readonly Guid DefaultOrganizationId = OrganizationModelFixture.DefaultOrganization.Id;
        private static readonly DateTime DefaultCreated = DateTime.UtcNow;
        private static readonly DateTime DefaultModified = DateTime.UtcNow;
        
        private const string DefaultAlias = "test-website";
        private const string DefaultDisplayName = "Test Website";
        
        public static ProjectEntity DefaultProject { get; } = new ProjectEntity(
            id: Guid.NewGuid(), 
            environmentId: DefaultEnvironmentId,
            alias: DefaultAlias,
            displayName: DefaultDisplayName,
            planId: DefaultPlanId,
            organizationId: DefaultOrganizationId,
            created: DefaultCreated,
            modified: DefaultModified
        );
    }
}