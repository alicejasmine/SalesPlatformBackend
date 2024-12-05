using ApplicationServices.Sample;
using Ardalis.ApiEndpoints;
using Domain.Sample;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Controllers;

public class CreateSampleEndpoint : EndpointBaseAsync.WithRequest<SampleDto>.WithActionResult
{
    private readonly ISampleService _sampleService;
    private readonly ILogger<CreateSampleEndpoint> _logger;

    public CreateSampleEndpoint(ISampleService sampleService)
    {
        _sampleService = sampleService;
    }

    [HttpPost("CreateSample")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Create a new Sample",
        Description = "Create a new sample entity",
        OperationId = "CreateSample",
        Tags = new[] { "Sample" })
    ]
    public override async Task<ActionResult> HandleAsync(
        [FromBody] SampleDto dto,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (!ModelState.IsValid)
        {
            return Problem(
                title: "Invalid Model",
                detail: "The provided sample data is not valid.",
                statusCode: 400
            );
        }

        try
        {
            await _sampleService.CreateSampleAsync(dto);
            return Ok();
        }
        
        catch (Exception ex)
        {
            return Problem(
                title: "Error Creating Sample",
                detail: ex.Message,
                statusCode: 500
            );
        }
    }
}
