using Domain.Enum;
using Domain.Models;

namespace TestFixtures.Organization;

public static class OrganizationModelFixture
{
    private static readonly Guid DefaultOrganizationId = Guid.Parse("c1aaf1b9-5824-4e27-9b29-3d5ebc5d66d4");
    private static readonly Guid NoCreditsOrganizationId = Guid.Parse("e5baf2b8-7a24-4b4d-9f28-4d5eac8e65a7");
    public static OrganizationModel DefaultOrganization { get; } = new OrganizationModel(
        id: DefaultOrganizationId,
        alias: "default-organization",
        displayName: "Default Organization",
        totalCredits: 850,
        partnership: PartnershipEnum.Silver,
        created: DateTime.UtcNow,
        modified: DateTime.UtcNow, 
        creditHistory: new List<CreditHistoryModel>
    {
        new CreditHistoryModel(
            id: Guid.NewGuid(),
            created: DateTime.UtcNow,
            modified: DateTime.UtcNow,
            invoiceNumber: "123456",
            partnershipCredits: 1000,
            creditsSpend: 100,
            currentCredits: 900,
            organizationId: DefaultOrganizationId
        ),
        new CreditHistoryModel(
            id: Guid.NewGuid(),
            created: DateTime.UtcNow,
            modified: DateTime.UtcNow,
            invoiceNumber: "123457",
            partnershipCredits: 1000,
            creditsSpend: 50,
            currentCredits: 850,
            organizationId: DefaultOrganizationId
        )
    }
    );
    
    public static OrganizationModel OrganizationWithoutCredits { get; } = new OrganizationModel(
        id: NoCreditsOrganizationId,
        alias: "no-credits-organization",
        displayName: "No Credits Organization",
        totalCredits: 0,
        partnership: PartnershipEnum.Platinum,
        created: DateTime.UtcNow,
        modified: DateTime.UtcNow,
        creditHistory: new List<CreditHistoryModel>() 
    );
}