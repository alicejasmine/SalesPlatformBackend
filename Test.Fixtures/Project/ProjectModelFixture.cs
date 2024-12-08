using Domain.Models;

namespace TestFixtures.Project;

public class ProjectModelFixture
{
    public static ProjectModel DefaultProject { get; } = new ProjectModel(
        id: Guid.NewGuid(),
        environmentId: Guid.Parse("a419a351-285f-4b7a-a262-ea2be1d5ee7d"),
        alias: "increa-website1",
        displayName: "Lego",
        planId: Guid.Parse("41788996-f724-48ee-9241-8ac057af0214"),
        organizationId: Guid.Parse("e13086cf-610d-4cce-adff-bb2e25f0f1cc"),
        created: DateTime.UtcNow,
        modified: DateTime.UtcNow
    );
}