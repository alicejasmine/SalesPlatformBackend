using ApplicationServices.Credit;
using Domain.Models;
using Infrastructure.Repositories.Credit;
using Moq;
using TestFixtures.Credits;
using TestFixtures.Organization;

namespace Tests.Services
{
    [TestFixture]
    public class CreditServiceTests
    {
        private Mock<ICreditRepository> _mockCreditRepository;
        private CreditService _creditService;

        [SetUp]
        public void SetUp()
        {
            _mockCreditRepository = new Mock<ICreditRepository>();
            _creditService = new CreditService(_mockCreditRepository.Object);
        }

        [Test]
        public async Task GetCreditHistoryByOrganizationAlias_ReturnsCreditHistory_WhenFound()
        {
            //Arrange
            var organizationAlias = OrganizationModelFixture.DefaultOrganization.Alias;
            var expectedCreditHistories = CreditHistoryModelFixture.DefaultCreditHistories;

            _mockCreditRepository.Setup(repo => repo.GetCreditHistoryByOrganizationAlias(organizationAlias))
                .ReturnsAsync(expectedCreditHistories);

            //Act
            var result = await _creditService.GetCreditHistoryByOrganizationAlias(organizationAlias);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedCreditHistories.Count));
            Assert.That(result[0].InvoiceNumber, Is.EqualTo(expectedCreditHistories[0].InvoiceNumber));
            Assert.That(result[1].InvoiceNumber, Is.EqualTo(expectedCreditHistories[1].InvoiceNumber));
            Assert.That(result[2].InvoiceNumber, Is.EqualTo(expectedCreditHistories[2].InvoiceNumber));
        }

        [Test]
        public async Task GetCreditHistoryByOrganizationAlias_ReturnsEmptyList_WhenNoCreditHistoryFound()
        {
            //Arrange
            var organizationAlias = OrganizationModelFixture.DefaultOrganization.Alias;

            _mockCreditRepository.Setup(repo => repo.GetCreditHistoryByOrganizationAlias(organizationAlias))
                .ReturnsAsync(new List<CreditHistoryModel>());

            //Act
            var result = await _creditService.GetCreditHistoryByOrganizationAlias(organizationAlias);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetCreditHistoryByOrganizationAlias_ThrowsException_WhenOrganizationAliasIsInvalid()
        {
            //Arrange
            var invalidAlias = string.Empty;

            _mockCreditRepository.Setup(repo => repo.GetCreditHistoryByOrganizationAlias(invalidAlias))
                .ThrowsAsync(new ArgumentException("OrganizationAlias cannot be null or empty"));

            //Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _creditService.GetCreditHistoryByOrganizationAlias(invalidAlias)
            );

            Assert.That(ex.Message, Is.EqualTo("OrganizationAlias cannot be null or empty"));
        }
    }
}
