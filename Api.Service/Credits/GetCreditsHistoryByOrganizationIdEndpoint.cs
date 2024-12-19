using Api.Service.Credits.DTOs;
using ApplicationServices.Credit;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Credits;

public class GetCreditsHistoryByOrganizationAliasEndpoint : EndpointBaseAsync.WithRequest<GetCreditsHistoryByOrganizationAliasRequestDto>.WithActionResult<List<CreditHistoryResponse>>
{
    private readonly ICreditService _creditService;

    public GetCreditsHistoryByOrganizationAliasEndpoint(ICreditService creditService)
    {
        _creditService = creditService;
    }

    [HttpGet("GetCreditsHistoryByOrganizationAlias")]
    [ProducesResponseType(typeof(CreditHistoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Get Credit History by Organization Alias",
        Description = "Retrieve the credit history for a specific organization based on its alias",
        OperationId = "GetCreditsHistoryByOrganizationAlias")
    ]
    public override async Task<ActionResult<List<CreditHistoryResponse>>> HandleAsync([FromQuery] GetCreditsHistoryByOrganizationAliasRequestDto dto, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var creditHistories = await _creditService.GetCreditHistoryByOrganizationAlias(dto.OrganizationAlias);

            if (creditHistories == null)
            {
                return Problem(
                    title: "Organization Not Found",
                    detail: $"No credit history found for the organization with alias '{dto.OrganizationAlias}'.",
                    statusCode: 404
                );
            }
            
            var response = creditHistories.Select(ch => new CreditHistoryResponse
            {
                InvoiceNumber=ch.InvoiceNumber,
                OrganizationId = ch.OrganizationId,
                OrganizationAlias = dto.OrganizationAlias,
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
