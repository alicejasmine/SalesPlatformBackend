using ApplicationServices.Sample;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Sample;

public class DeleteSampleEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithActionResult
{
    private readonly ISampleService _sampleService;

    public DeleteSampleEndpoint(ISampleService sampleService, ILogger<DeleteSampleEndpoint> logger)
    {
        _sampleService = sampleService;
    }

    [HttpDelete("DeleteSample")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Delete a Sample",
        Description = "Delete a sample entity by its ID",
        OperationId = "DeleteSample",
        Tags = new[] { "Sample" })
    ]
    public override async Task<ActionResult> HandleAsync(
        Guid id,
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            await _sampleService.DeleteSampleAsync(id);

            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Error Deleting Sample",
                detail: "Could not delete the sample. Please check the provided ID and try again.",
                statusCode: 500
            );
        }
    }
}