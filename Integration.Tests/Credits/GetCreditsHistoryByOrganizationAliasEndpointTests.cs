using System.Net;
using System.Net.Http.Json;
using Api.Service.Credits;
using Api.Service.Credits.DTOs;
using Integration.Tests.Library;
using Microsoft.AspNetCore.Mvc;
using TestFixtures.Credits;
using TestFixtures.Organization;

namespace Integration.Tests.Credits;

[TestFixture]
[TestOf(typeof(GetCreditsHistoryByOrganizationAliasEndpoint))]
internal sealed class GetCreditsHistoryByOrganizationAliasEndpointTests : BaseEndpointTests
{
    [Test]
    public async Task GetCreditsHistoryByOrganizationAlias_ReturnsNotFound_WhenOrganizationNotFound()
    {
        //Arrange
        var organizationAlias = "nonexistent-alias";

        //Act
        using var response = await AppHttpClient.GetAsync($"/GetCreditsHistoryByOrganizationAlias?organizationAlias={organizationAlias}");

        //Assert
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.That(problemDetails, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(problemDetails!.Title, Is.EqualTo("Organization Not Found"));
        Assert.That(problemDetails.Detail, Does.Contain(organizationAlias));
    }

    [Test]
    public async Task GetCreditsHistoryByOrganizationAlias_ReturnsCreditHistory_WhenSuccess()
    {
        //Arrange
        var organization = OrganizationModelFixture.DefaultOrganization;
        var creditHistory = CreditHistoryModelFixture.DefaultCreditHistories;
        await Data.StoreOrganization(organization);
        await Data.StoreCreditHistory(creditHistory);

        //Act
        using var response = await AppHttpClient.GetAsync($"/GetCreditsHistoryByOrganizationAlias?organizationAlias={organization.Alias}");

        //Assert
        var creditHistoryResponse = await response.Content.ReadFromJsonAsync<List<CreditHistoryResponse>>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(creditHistoryResponse, Is.Not.Null);
        Assert.That(creditHistoryResponse!.Count, Is.GreaterThan(0));

        var history = creditHistoryResponse.First();
        var expectedHistory = creditHistory.First(); 

        Assert.That(history.OrganizationAlias, Is.EqualTo(organization.Alias));
        Assert.That(history.OrganizationId, Is.EqualTo(organization.Id));
        Assert.That(history.TotalPartnershipCredits, Is.EqualTo(expectedHistory.PartnershipCredits));
        Assert.That(history.CreditsSpent, Is.EqualTo(expectedHistory.CreditsSpend));
        Assert.That(history.RemainingCredits, Is.EqualTo(expectedHistory.PartnershipCredits - expectedHistory.CreditsSpend)); // Correct remaining credits calculation
        Assert.That(history.Created, Is.EqualTo(expectedHistory.Created.ToString("dd-MM-yyyy")));
        Assert.That(history.Modified, Is.EqualTo(expectedHistory.Modified.ToString("dd-MM-yyyy")));
    }
}
