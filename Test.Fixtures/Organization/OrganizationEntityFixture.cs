using Domain.Entities;
using Domain.Enum;

namespace TestFixtures.Organization
{
    public static class OrganizationEntityFixture
    {
        public static OrganizationEntity DefaultOrganization
        {
            get
            {
                return new OrganizationEntity(
                    id: OrganizationModelFixture.DefaultOrganization.Id,
                    alias: "oxygen",
                    displayName: "Oxygen",
                    totalCredits: 154827,
                    partnership: PartnershipEnum.Gold,
                    created: DateTime.UtcNow,
                    modified: DateTime.UtcNow,
                    creditHistories:GetDefaultCreditHistories()
                );
            }
        }

        public static List<OrganizationEntity> GetOrganizations()
        {
            return new List<OrganizationEntity>
            {
                new OrganizationEntity(
                    id: Guid.NewGuid(),
                    alias: "oxygen",
                    displayName: "Oxygen",
                    totalCredits: 154827,
                    partnership: PartnershipEnum.Gold,
                    created: DateTime.UtcNow,
                    modified: DateTime.UtcNow,
                    creditHistories: GetCreditHistoriesForOxygen()
                ),
                new OrganizationEntity(
                    id: Guid.NewGuid(),
                    alias: "io",
                    displayName: "iO",
                    totalCredits: 789456,
                    partnership: PartnershipEnum.Platinum,
                    created: DateTime.UtcNow,
                    modified: DateTime.UtcNow,
                    creditHistories: GetCreditHistoriesForIO()
                ),
                new OrganizationEntity(
                    id: Guid.NewGuid(),
                    alias: "increo",
                    displayName: "Increo",
                    totalCredits: 784,
                    partnership: PartnershipEnum.Silver,
                    created: DateTime.UtcNow,
                    modified: DateTime.UtcNow,
                    creditHistories: GetCreditHistoriesForIncreo()
                )
            };
        }
        
        private static List<CreditHistoryEntity> GetDefaultCreditHistories()
        {
            return new List<CreditHistoryEntity>
            {
                new CreditHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow.AddMonths(-2),
                    Modified = DateTime.UtcNow,
                    InvoiceNumber = "12345",
                    PartnershipCredits = 5000,
                    CreditsSpend = 2000,
                    CurrentCredits = 3000,
                    OrganizationId = OrganizationModelFixture.DefaultOrganization.Id
                },
                new CreditHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow.AddMonths(-1),
                    Modified = DateTime.UtcNow,
                    InvoiceNumber = "67890",
                    PartnershipCredits = 3000,
                    CreditsSpend = 1500,
                    CurrentCredits = 1500,
                    OrganizationId = OrganizationModelFixture.DefaultOrganization.Id
                }
            };
        }
        
        private static List<CreditHistoryEntity> GetCreditHistoriesForIO()
        {
            return new List<CreditHistoryEntity>
            {
                new CreditHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow.AddMonths(-3),
                    Modified = DateTime.UtcNow,
                    InvoiceNumber = "22222",
                    PartnershipCredits = 10000,
                    CreditsSpend = 5000,
                    CurrentCredits = 5000,
                    OrganizationId = Guid.NewGuid() 
                }
            };
        }

        private static List<CreditHistoryEntity> GetCreditHistoriesForIncreo()
        {
            return new List<CreditHistoryEntity>
            {
                new CreditHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow.AddMonths(-4),
                    Modified = DateTime.UtcNow,
                    InvoiceNumber = "33333",
                    PartnershipCredits = 1000,
                    CreditsSpend = 500,
                    CurrentCredits = 500,
                    OrganizationId = Guid.NewGuid() 
                }
            };
        }
        
        private static List<CreditHistoryEntity> GetCreditHistoriesForOxygen()
        {
            return new List<CreditHistoryEntity>
            {
                new CreditHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow.AddMonths(-4),
                    Modified = DateTime.UtcNow,
                    InvoiceNumber = "33333",
                    PartnershipCredits = 1000,
                    CreditsSpend = 500,
                    CurrentCredits = 500,
                    OrganizationId = Guid.NewGuid() 
                }
            };
        }
    }
}


