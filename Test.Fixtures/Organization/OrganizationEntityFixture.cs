using Domain.Entities;
using Domain.Enum;

namespace TestFixtures.Organization;

public static class OrganizationEntityFixture
{
    public static OrganizationEntity DefaultOrganization { get; } = new OrganizationEntity(
        id: Guid.Parse("e13086cf-610d-4cce-adff-bb2e25f0f1cc"),
        alias: "default-organization",
        displayName: "Default Organization",
        totalCredits: 1000,
        partnership: PartnershipEnum.Silver,
        created: DateTime.UtcNow,
        modified: DateTime.UtcNow)
    {
        Projects = new List<ProjectEntity>()
    };
}