using Api.Service.Usage.DTOs;
using ApplicationServices.Usage;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;
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
    [ProducesResponseType(typeof(YearTotalUsageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get Year Worth Of Usage",
        Description = "Get Year worth of usage data by alias",
        OperationId = "GetYearWorthOfTotalDatasUsage")
        ]
    public override async Task<ActionResult<YearTotalUsageResponse>> HandleAsync([FromQuery] GetYearWorthOfTotalDataRequestDto requestDto, CancellationToken cancellationToken = new())
    {
        var currentDate = DateTime.UtcNow;
        var usageData = await _usageDocumentService.GetYearOfUsageData(requestDto.Alias, requestDto.year);

        if (usageData.totalBandwidthInBytes == 0 && usageData.totalMediaInBytes == 0)
        {
            return Problem(
                title: "Usage data not found",
                detail: $"No usage data found for alias {requestDto.Alias} starting from {currentDate.Month}/{currentDate.Year}",
                statusCode: StatusCodes.Status404NotFound);
        }

        var response = new YearTotalUsageResponse
        {
            TotalBandwidth = usageData.totalBandwidthInBytes,
            TotalMedia = usageData.totalMediaInBytes
        };

        return Ok(response);
    }
}