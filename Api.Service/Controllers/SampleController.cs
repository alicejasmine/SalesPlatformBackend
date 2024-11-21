using Microsoft.AspNetCore.Mvc;
using ApplicationServices.Sample;
using Domain.Sample;
using System.Collections.Immutable;

namespace Api.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;
    private readonly ISampleService _sampleService;

    public SampleController(ILogger<SampleController> logger, ISampleService sampleService)
    {
        _logger = logger;
        _sampleService = sampleService;
    }

    [HttpGet("GetSample")]
    public async Task<ActionResult<SampleModel>> Get(Guid id)
    {
        var sample = await _sampleService.GetSampleByIdAsync(id);

        return Ok(sample);
    }

    [HttpGet("/GetAllSample")]
    public async Task<ActionResult<ImmutableHashSet<SampleDto>>> GetAll()
    {
        var samples = await _sampleService.GetAllSamplesAsync();

        return new ActionResult<ImmutableHashSet<SampleDto>>(samples.ToImmutableHashSet());
    }

    [HttpPost("CreateSample")]
    public async Task<ActionResult> Create([FromBody] SampleDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        try
        {
            await _sampleService.CreateSampleAsync(model);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"{ex.Message}", ex);
        }

        _logger.LogInformation("Created new SampleModel with ID: {Id}", model.Id);

        return Ok();
    }

    [HttpDelete("/DeleteSample")]
    public async Task<IActionResult> HandleDelete(Guid id)
    {
        try
        {
            await _sampleService.DeleteSampleAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"{ex.Message}", ex);
            return BadRequest("Could not delete the data");
        }
        return Ok();
    }
    
    [HttpPut("/UpdateSample")]
    public async Task<ActionResult> Update([FromBody] SampleDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }
        
        try
        {
            var existingSample = await _sampleService.GetSampleByIdAsync(model.Id);
            if (existingSample == null)
            {
                return NotFound($"Sample with ID {model.Id} not found.");
            }
            
            await _sampleService.UpdateSampleAsync(model);

            _logger.LogInformation("Updated SampleModel with ID: {Id}", model.Id);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SampleModel with ID: {Id}", model.Id);
            return StatusCode(500, "Internal server error");
        }
    }

}