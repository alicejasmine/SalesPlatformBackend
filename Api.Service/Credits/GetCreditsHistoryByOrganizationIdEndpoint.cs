using Api.Service.Credits.DTOs;
using ApplicationServices.Credit;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Credits;

public class GetCreditsHistoryByOrganizationAliasEndpoint : EndpointBaseAsync.WithRequest<string>.WithActionResult<CreditHistoryResponse>
{
    private readonly ICreditService _creditService;

    public GetCreditsHistoryByOrganizationAliasEndpoint(ICreditService creditService)
    {
        _creditService = creditService;
    }

    [HttpGet("GetCreditsHistoryByAlias/{organizationAlias}")]
    [ProducesResponseType(typeof(CreditHistoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Get Credit History by Organization Alias",
        Description = "Retrieve the credit history for a specific organization based on its alias",
        OperationId = "GetCreditsHistoryByOrganizationAlias")
    ]
    public override async Task<ActionResult<CreditHistoryResponse>> HandleAsync(string organizationAlias, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var creditHistories = await _creditService.GetCreditHistoryByOrganizationAlias(organizationAlias);

            if (creditHistories == null)
            {
                return Problem(
                    title: "Organization Not Found",
                    detail: $"No credit history found for the organization with alias '{organizationAlias}'.",
                    statusCode: 404
                );
            }
            
            var response = creditHistories.Select(ch => new CreditHistoryResponse
            {
                InvoiceNumber=ch.InvoiceNumber,
                OrganizationId = ch.OrganizationId,
                OrganizationAlias = organizationAlias,
                TotalPartnershipCredits = ch.PartnershipCredits,
                CreditsSpent = ch.CreditsSpend,
                RemainingCredits = ch.CurrentCredits,
                Created = ch.Created.ToString("dd-MM-yyyy"),
                Modified = ch.Modified.ToString("dd-MM-yyyy")
            }).ToList();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Error Retrieving Credit History",
                detail: ex.Message,
                statusCode: 500
            );
        }
    }
}
