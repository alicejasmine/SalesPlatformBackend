using Infrastructure.Repositories.Credit;
using Infrastructure.Repositories.Organization;
using Integration.Tests.Library;
using TestFixtures.Credits;
using TestFixtures.Organization;

namespace Integration.Tests.Credits
{
    [TestFixture]
    internal sealed class CreditRepositoryTests : BaseDatabaseTestFixture
    {
        private CreditRepository _creditRepository;
        private OrganizationRepository _organizationRepository;

        [SetUp]
        public async Task SetUp()
        {
            _creditRepository = new CreditRepository(DatabaseTestsFixture.DbContext);
            _organizationRepository = new OrganizationRepository(DatabaseTestsFixture.DbContext, _creditRepository);
        }

        [Test]
        public async Task GetCreditHistoryByOrganizationAlias_ReturnsCreditHistory_WhenFound()
        {
            // Arrange
            var organization = OrganizationModelFixture.DefaultOrganization;
            await _organizationRepository.UpsertAsync(organization);
            
            var expectedCreditHistory = CreditHistoryModelFixture.DefaultCreditHistories;
            await _creditRepository.UpsertAsync(expectedCreditHistory[0]);
            await _creditRepository.UpsertAsync(expectedCreditHistory[1]);

            // Act
            var creditHistories = await _creditRepository.GetCreditHistoryByOrganizationAlias(organization.Alias);

            // Assert
            Assert.That(creditHistories, Is.Not.Null);
            Assert.That(creditHistories.Count, Is.GreaterThan(0));
            Assert.That(creditHistories[0].InvoiceNumber, Is.EqualTo(expectedCreditHistory.First().InvoiceNumber));
            Assert.That(creditHistories[1].InvoiceNumber, Is.EqualTo(expectedCreditHistory[1].InvoiceNumber));
        }

        [Test]
        public async Task GetCreditHistoryByOrganizationAlias_ReturnsEmptyList_WhenNoCreditHistoryFound()
        {
            // Arrange
            var organization = OrganizationModelFixture.OrganizationWithoutCredits;
            await _organizationRepository.UpsertAsync(organization);

            // Act
            var creditHistories = await _creditRepository.GetCreditHistoryByOrganizationAlias(organization.Alias);

            // Assert
            Assert.That(creditHistories, Is.Not.Null);
            Assert.That(creditHistories, Is.Empty);
        }

        [Test]
        public async Task GetCreditHistoryByOrganizationAlias_ThrowsKeyNotFoundException_WhenOrganizationNotFound()
        {
            //Arrange
            var nonExistentAlias = "non-existent-alias";

            //Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _creditRepository.GetCreditHistoryByOrganizationAlias(nonExistentAlias)
            );
            Assert.That(ex.Message, Is.EqualTo($"No organization found with alias '{nonExistentAlias}'."));
        }

        [Test]
        public async Task UpsertAsync_UpdatesCreditHistory_WhenExisting()
        {
            //Arrange
            var organization = OrganizationModelFixture.DefaultOrganization;
            await _organizationRepository.UpsertAsync(organization);

            var initialCreditHistory = CreditHistoryModelFixture.DefaultCreditHistories;
            await _creditRepository.UpsertAsync(initialCreditHistory.First());
            await _creditRepository.UpsertAsync(initialCreditHistory[1]);
            
            initialCreditHistory.First().CreditsSpend = 100;
            await _creditRepository.UpsertAsync(initialCreditHistory.First());

            //Act
            var updatedCreditHistory = await _creditRepository.GetCreditHistoryByOrganizationAlias(organization.Alias);

            //Assert
            Assert.That(updatedCreditHistory, Is.Not.Null);
            Assert.That(updatedCreditHistory[0].CreditsSpend, Is.EqualTo(100));
        }

        [Test]
        public async Task UpsertAsync_AddsCreditHistory_WhenNew()
        {
            //Arrange
            var organization = OrganizationModelFixture.DefaultOrganization;
            await _organizationRepository.UpsertAsync(organization);

            var newCreditHistory = CreditHistoryModelFixture.DefaultCreditHistories;

            //Act
            await _creditRepository.UpsertAsync(newCreditHistory.First());

            var creditHistories = await _creditRepository.GetCreditHistoryByOrganizationAlias(organization.Alias);

            //Assert
            Assert.That(creditHistories, Is.Not.Null);
            Assert.That(creditHistories.Count, Is.EqualTo(3));
            Assert.That(creditHistories[0].InvoiceNumber, Is.EqualTo(newCreditHistory.First().InvoiceNumber));
        }

        [Test]
        public void GetCreditHistoryByOrganizationAlias_ThrowsException_WhenOrganizationAliasIsEmpty()
        {
            //Arrange
            var invalidAlias = "";

            //Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _creditRepository.GetCreditHistoryByOrganizationAlias(invalidAlias)
            );
            Assert.That(ex.Message, Is.EqualTo("OrganizationAlias cannot be null or empty"));
        }

        [Test]
        public void GetCreditHistoryByOrganizationAlias_ThrowsException_WhenOrganizationAliasIsNull()
        {
            //Arrange
            string? invalidAlias = null;

            //Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _creditRepository.GetCreditHistoryByOrganizationAlias(invalidAlias)
            );
            Assert.That(ex.Message, Is.EqualTo("OrganizationAlias cannot be null or empty"));
        }
    }
}
