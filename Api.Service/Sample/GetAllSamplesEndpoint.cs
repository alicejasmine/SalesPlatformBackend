using System.Collections.Immutable;
using ApplicationServices.Sample;
using Ardalis.ApiEndpoints;
using Domain.Sample;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Sample;

public class GetAllSamplesEndpoint : EndpointBaseAsync.WithoutRequest.WithActionResult<ImmutableHashSet<SampleModel>>
{
    private readonly ISampleService _sampleService;

    public GetAllSamplesEndpoint(ISampleService sampleService)
    {
        _sampleService = sampleService;
    }

    [HttpGet("GetAllSamples")]
    [ProducesResponseType(typeof(ImmutableHashSet<SampleModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Get All Samples",
        Description = "Retrieve all samples as an immutable set",
        OperationId = "GetAllSamples",
        Tags = new[] { "Sample" })
    ]
    public override async Task<ActionResult<ImmutableHashSet<SampleModel>>> HandleAsync(
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var samples = await _sampleService.GetAllSamplesAsync();
            return Ok(samples.ToImmutableHashSet());
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Error Retrieving Samples",
                detail: ex.Message,
                statusCode: 500
            );
        }
    }
}