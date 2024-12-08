using Domain.Enum;
using Domain.Models;

namespace TestFixtures.Organization;

public static class OrganizationModelFixture
{
    public static OrganizationModel DefaultOrganization { get; } = new OrganizationModel(
        id: Guid.Parse("f7d3a2a9-50d8-4bcd-b1aa-e3c9f30347e7"),
        alias: "default-organization",
        displayName: "Default Organization",
        totalCredits: 1000,
        partnership: PartnershipEnum.Silver,
        created: DateTime.UtcNow,
        modified: DateTime.UtcNow
    );
}