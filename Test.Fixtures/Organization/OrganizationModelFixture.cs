using Domain.Enum;
using Domain.Models;

namespace TestFixtures.Organization;

public static class OrganizationModelFixture
{
    private static readonly Guid DefaultOrganizationId = Guid.Parse("c1aaf1b9-5824-4e27-9b29-3d5ebc5d66d4");
    public static OrganizationModel DefaultOrganization { get; } = new OrganizationModel(
        id: DefaultOrganizationId,
        alias: "default-organization",
        displayName: "Default Organization",
        totalCredits: 1000,
        partnership: PartnershipEnum.Silver,
        created: DateTime.UtcNow,
        modified: DateTime.UtcNow
    );
}