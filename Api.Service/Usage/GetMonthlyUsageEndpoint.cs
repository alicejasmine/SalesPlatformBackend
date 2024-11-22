using Api.Service.Controllers;
using Api.Service.DTOs;
using ApplicationServices;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Usage;

public class GetMonthlyUsageEndpoint : EndpointBaseAsync.WithRequest<GetMonthlyUsageRequestDto>.WithActionResult<UsageResponse>
{
    private readonly IUsageDocumentService _usageDocumentService;

    public GetMonthlyUsageEndpoint(IUsageDocumentService usageDocumentService)
    {
        _usageDocumentService = usageDocumentService;
      
    }

    [HttpGet("GetMonthlyUsage/{environmentId}/{year}/{month}")]
    [ProducesResponseType(typeof(UsageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get Monthly Usage",
        Description = "Get Monthly Usage by environment id and date",
        OperationId = "GetMonthlyUsage")
    ]
    
    public override async Task<ActionResult<UsageResponse>> HandleAsync(GetMonthlyUsageRequestDto dto, CancellationToken cancellationToken = new CancellationToken())
    {
        var monthlyUsage = await _usageDocumentService.GetUsageEntity(dto.EnvironmentId, dto.Month, dto.Year);
        if (monthlyUsage == null)
        {
            return Problem(
                title: "Usage data could not be found for the selected date",
                detail: $"Could not find usage data with environmentId {dto.EnvironmentId} for the month {dto.Year} and year {dto.Year}",
                statusCode: 404
            ); 
        }

        return new ActionResult<UsageResponse>(UsageMapper.MapEntityToResponse(monthlyUsage, dto.Year, dto.Month));
    }
}