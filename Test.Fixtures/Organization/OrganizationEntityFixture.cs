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
                    id: Guid.NewGuid(),
                    alias: "oxygen",
                    displayName: "Oxygen",
                    totalCredits: 154827,
                    partnership: PartnershipEnum.Gold,
                    created: DateTime.UtcNow,
                    modified: DateTime.UtcNow
                );
            }
        }

        public static OrganizationEntity CustomOrganization(string alias, string displayName, int totalCredits, PartnershipEnum partnership)
        {
            return new OrganizationEntity(
                id: Guid.NewGuid(),
                alias: alias,
                displayName: displayName,
                totalCredits: totalCredits,
                partnership: partnership,
                created: DateTime.UtcNow,
                modified: DateTime.UtcNow
            );
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
                    modified: DateTime.UtcNow
                ),
                new OrganizationEntity(
                    id: Guid.NewGuid(),
                    alias: "io",
                    displayName: "iO",
                    totalCredits: 789456,
                    partnership: PartnershipEnum.Platinum,
                    created: DateTime.UtcNow,
                    modified: DateTime.UtcNow
                ),
                new OrganizationEntity(
                    id: Guid.NewGuid(),
                    alias: "increo",
                    displayName: "Increo",
                    totalCredits: 784,
                    partnership: PartnershipEnum.Silver,
                    created: DateTime.UtcNow,
                    modified: DateTime.UtcNow
                )
            };
        }
    }
}
