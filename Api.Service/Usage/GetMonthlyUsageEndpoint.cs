using Api.Service.DTOs;
using Api.Service.Usage.DTOs;
using ApplicationServices.Usage;
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
    
    [HttpGet("Usage/GetMonthlyUsage")]
    [ProducesResponseType(typeof(UsageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get Monthly Usage",
        Description = "Get Monthly Usage by project alias and date",
        OperationId = "GetMonthlyUsage")
    ]
    
    public override async Task<ActionResult<UsageResponse>> HandleAsync([FromQuery] GetMonthlyUsageRequestDto dto, CancellationToken cancellationToken = new CancellationToken())
    {
        var monthlyUsage = await _usageDocumentService.GetUsageEntity(dto.Alias, dto.Month, dto.Year);

        if (monthlyUsage == null)
        {
            return Problem(
                title: "Usage data could not be found for the selected date",
                detail: $"Could not find usage data with Alias {dto.Alias} for the month {dto.Month} and year {dto.Year}",
                statusCode: 404
            ); 
        }

        return new ActionResult<UsageResponse>(UsageMapper.MapEntityToResponse(monthlyUsage, dto.Year, dto.Month));
    }
}