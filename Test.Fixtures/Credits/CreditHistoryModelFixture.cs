using Domain.Models;
using TestFixtures.Organization;

namespace TestFixtures.Credits;

public static class CreditHistoryModelFixture
{
    private static readonly Guid DefaultOrganizationId = OrganizationModelFixture.DefaultOrganization.Id;
    
 public static List<CreditHistoryModel> DefaultCreditHistories { get; } = new List<CreditHistoryModel>
    {
        new CreditHistoryModel(
            id: Guid.NewGuid(),
            created: DateTime.UtcNow,
            modified: DateTime.UtcNow,
            invoiceNumber: "123456",
            partnershipCredits: 500,
            creditsSpend: 100,
            currentCredits: 400,
            organizationId: DefaultOrganizationId
        ),
        new CreditHistoryModel(
            id: Guid.NewGuid(),
            created: DateTime.UtcNow.AddDays(-5),
            modified: DateTime.UtcNow.AddDays(-5),
            invoiceNumber: "123457",
            partnershipCredits: 300,
            creditsSpend: 50,
            currentCredits: 250,
            organizationId: DefaultOrganizationId
        ),
        new CreditHistoryModel(
            id: Guid.NewGuid(),
            created: DateTime.UtcNow.AddDays(-10),
            modified: DateTime.UtcNow.AddDays(-10),
            invoiceNumber: "123458",
            partnershipCredits: 200,
            creditsSpend: 30,
            currentCredits: 170,
            organizationId: DefaultOrganizationId
        )
    };
}