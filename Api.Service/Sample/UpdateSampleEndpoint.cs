using ApplicationServices.Sample;
using Ardalis.ApiEndpoints;
using Domain.Sample;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Sample;

public class UpdateSampleEndpoint : EndpointBaseAsync.WithRequest<SampleDto>.WithActionResult
{
    private readonly ISampleService _sampleService;

    public UpdateSampleEndpoint(ISampleService sampleService)
    {
        _sampleService = sampleService;
    }

    [HttpPut("UpdateSample")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Update a Sample",
        Description = "Update an existing sample entity",
        OperationId = "UpdateSample",
        Tags = new[] { "Sample" })
    ]
    public override async Task<ActionResult> HandleAsync(SampleDto dto, CancellationToken cancellationToken = new CancellationToken())
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var existingSample = await _sampleService.GetSampleByIdAsync(dto.Id);
            if (existingSample == null)
            {
                return Problem(
                    title: "Sample Not Found",
                    detail: $"Sample with ID {dto.Id} not found.",
                    statusCode: 404
                );
            }

            await _sampleService.UpdateSampleAsync(dto);

            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Error Updating Sample",
                detail: "An error occurred while updating the sample.",
                statusCode: 500
            );
        }
    }
}