using Api.Service.Controllers;
using Api.Service.DTOs;
using Api.Service.Usage.DTOs;
using ApplicationServices.Usage;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Usage;

public class GetYearWorthOfTotalDataEndpoint : EndpointBaseAsync
    .WithRequest<GetYearWorthOfTotalDataRequestDto>
    .WithActionResult<YearTotalUsageResponse>
{
    private readonly IUsageDocumentService _usageDocumentService;

    public GetYearWorthOfTotalDataEndpoint(IUsageDocumentService usageDocumentService)
    {
        _usageDocumentService = usageDocumentService;
    }

    [HttpGet("Usage/GetYearWorthOfTotalData")]
    [ProducesResponseType(typeof((long totalBandwidth, long totalMedia)), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get Year Worth Of Usage",
        Description = "Get Year worth of usage data by id and date",
        OperationId = "GetSixMonthGetYearWorthOfTotalDatasUsage")
        ]

    public override async Task<ActionResult<YearTotalUsageResponse>> HandleAsync([FromQuery] GetYearWorthOfTotalDataRequestDto requestDto, CancellationToken cancellationToken = new())
    {
        var currentDate = DateTime.UtcNow;
        var usageData = await _usageDocumentService.GetUsageEntitiesForMultipleMonths(requestDto.EnvironmentId, currentDate.Month, currentDate.Year, 12);

        if (usageData == null || !usageData.Any())
        {
            return Problem(
                title: "Usage data not found",
                detail: $"No usage data found for environmentId {requestDto.EnvironmentId} starting from {currentDate.Month}/{currentDate.Year}",
                statusCode: StatusCodes.Status404NotFound);
        }

        var totalBandwidth = usageData.Sum(u => u.TotalMonthlyBandwidth);
        var totalMedia = usageData.Sum(u => u.TotalMonthlyMedia);

        var response = new YearTotalUsageResponse
        {
            TotalBandwidth = totalBandwidth,
            TotalMedia = totalMedia
        };

        return Ok(response);
    }
}