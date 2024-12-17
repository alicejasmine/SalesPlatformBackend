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
        modified: DateTime.UtcNow, creditHistory: new List<CreditHistoryModel>
    {
        new CreditHistoryModel(
            id: Guid.NewGuid(),
            created: DateTime.UtcNow,
            modified: DateTime.UtcNow,
            invoiceNumber: 123456,
            partnershipCredits: 500,
            creditsSpend: 100,
            currentCredits: 400,
            creditStart: new DateOnly(2024, 1, 1),
            creditEnd: new DateOnly(2024, 12, 31),
            organizationId: DefaultOrganizationId
        ),
        new CreditHistoryModel(
            id: Guid.NewGuid(),
            created: DateTime.UtcNow,
            modified: DateTime.UtcNow,
            invoiceNumber: 123457,
            partnershipCredits: 300,
            creditsSpend: 50,
            currentCredits: 250,
            creditStart: new DateOnly(2024, 2, 1),
            creditEnd: new DateOnly(2024, 12, 31),
            organizationId: DefaultOrganizationId
        )
    }
    );
}