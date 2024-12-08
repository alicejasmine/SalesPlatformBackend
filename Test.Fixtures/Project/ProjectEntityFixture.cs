using Domain.Entities;

namespace TestFixtures.Project;

public class ProjectEntityFixture
{
    public static ProjectEntity DefaultProject { get; } = new ProjectEntity
    {
        Id = Guid.NewGuid(), 
        Alias = "default-website1",
        DisplayName = "default project",
        EnvironmentId = Guid.Parse("a419a351-285f-4b7a-a262-ea2be1d5ee7d"),
        PlanId = Guid.Parse("41788996-f724-48ee-9241-8ac057af0214"),
        OrganizationId = Guid.Parse("f7d3a2a9-50d8-4bcd-b1aa-e3c9f30347e7"),
        Created = DateTime.UtcNow,
        Modified = DateTime.UtcNow
    };
}