using ApplicationServices.Sample;
using Ardalis.ApiEndpoints;
using Domain.Sample;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Sample;

public class GetSampleEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithActionResult<SampleModel>
{
    private readonly ISampleService _sampleService;

    public GetSampleEndpoint(ISampleService sampleService)
    {
        _sampleService = sampleService;
    }

    [HttpGet("GetSample")]
    [ProducesResponseType(typeof(SampleModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get Sample by ID",
        Description = "Retrieve a sample entity by its ID",
        OperationId = "GetSample",
        Tags = new[] { "Sample" })
    ]
    public override async Task<ActionResult<SampleModel>> HandleAsync(
        [FromQuery] Guid id,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var sample = await _sampleService.GetSampleByIdAsync(id);

        if (sample == null)
        {
            return Problem(
                title: "Sample Not Found",
                detail: $"No sample found with ID {id}",
                statusCode: 404
            );
        }

        return Ok(sample);
    }
}