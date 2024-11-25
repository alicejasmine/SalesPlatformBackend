using Api.Service.Controllers;
using Api.Service.DTOs;
using Api.Service.Usage.DTOs;
using ApplicationServices.Usage;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Usage;
public class GetSixMonthsUsageEndpoint : EndpointBaseAsync.WithRequest<GetMonthlyUsageRequestDto>.WithActionResult<UsageTotalUsageResponse>
{
    private readonly IUsageDocumentService _usageDocumentService;

    public GetSixMonthsUsageEndpoint(IUsageDocumentService usageDocumentService)
    {
        _usageDocumentService = usageDocumentService;
    }

    [HttpGet("GetSixMonthsUsage")]
    [ProducesResponseType(typeof(UsageTotalUsageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get Six Months Usage",
        Description = "Get total bandwidth and media usage for the last 6 months by environment id and date",
        OperationId = "GetSixMonthsUsage")
        ]

    public override async Task<ActionResult<UsageTotalUsageResponse>> HandleAsync([FromQuery] GetMonthlyUsageRequestDto dto, CancellationToken cancellationToken = new())
    {
        var usageData = await _usageDocumentService.GetUsageEntities(dto.EnvironmentId, dto.Month, dto.Year, 6);

        if (usageData == null || !usageData.Any())
        {
            return Problem(
                title: "Usage data not found",
                detail: $"No usage data found for environmentId {dto.EnvironmentId} starting from {dto.Month}/{dto.Year}",
                statusCode: StatusCodes.Status404NotFound);
        }

        var totalBandwidth = usageData.Sum(u => u.TotalMonthlyBandwidth);
        var totalMedia = usageData.Sum(u => u.TotalMonthlyMedia);

        return new ActionResult<UsageTotalUsageResponse>(UsageMapper.MapTotalUsageEntityToResponse(totalBandwidth, totalMedia));
    }
}