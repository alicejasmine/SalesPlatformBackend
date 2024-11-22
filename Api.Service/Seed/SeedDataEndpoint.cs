using ApplicationServices.Seed;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Seed
{
    public class SeedDataEndpoint : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly ISeedService _seedService;

        public SeedDataEndpoint(ISeedService seedService)
        {
            _seedService = seedService;
        }

        [HttpGet("GetMonthlyUsage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Get Monthly Usage",
            Description = "Get Monthly Usage by environment id and date",
            OperationId = "GetMonthlyUsage")
            ]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await _seedService.SeedDatabasesWithData();
            }
            catch (Exception ex)
            {
                return Problem(
                title: "Could not seed data into databases correctly",
                detail: $"could not seed data properly in the databases, error {ex}",
                statusCode: 404
                );
            }
            return Ok();
        }
    }
}
