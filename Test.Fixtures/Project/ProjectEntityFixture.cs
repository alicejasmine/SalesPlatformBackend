using Domain.Entities;

namespace TestFixtures.Project;

public class ProjectEntityFixture
{
    public static ProjectEntity DefaultProject { get; } = new ProjectEntity
    {
        Id = Guid.NewGuid(), 
        Alias = "increa-website1",
        DisplayName = "Lego",
        EnvironmentId = Guid.Parse("a419a351-285f-4b7a-a262-ea2be1d5ee7d"),
        PlanId = Guid.Parse("41788996-f724-48ee-9241-8ac057af0214"),
        OrganizationId = Guid.Parse("e13086cf-610d-4cce-adff-bb2e25f0f1cc"),
        Created = DateTime.UtcNow,
        Modified = DateTime.UtcNow
    };
}