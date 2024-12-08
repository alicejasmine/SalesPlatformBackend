using Domain.Models;

namespace TestFixtures.Project;

public class ProjectModelFixture
{
    public static ProjectModel DefaultProject { get; } = new ProjectModel(
        id: Guid.NewGuid(),
        environmentId: Guid.Parse("a419a351-285f-4b7a-a262-ea2be1d5ee7d"),
        alias: "default-website1",
        displayName: "default project",
        planId: Guid.Parse("41788996-f724-48ee-9241-8ac057af0214"),
        organizationId: Guid.Parse("f7d3a2a9-50d8-4bcd-b1aa-e3c9f30347e7"),
        created: DateTime.UtcNow,
        modified: DateTime.UtcNow
    );
}