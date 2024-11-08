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

    [HttpGet("{id:guid}", Name = "GetSample")]
    public async Task<ActionResult<SampleModel>> Get(Guid id)
    {
        var sample = await _sampleService.GetSampleByIdAsync(id);

        return Ok(sample);
    }

    [HttpGet("all", Name = "GetAllSamples")]
    public async Task<ActionResult<ImmutableHashSet<SampleDto>>> GetAll()
    {
        var samples = await _sampleService.GetAllSamplesAsync();

        return new ActionResult<ImmutableHashSet<SampleDto>>(samples.ToImmutableHashSet());
    }

    [HttpPost(Name = "CreateSample")]
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
            _logger.LogInformation("Could not create new sample in database: {ex}", ex);
        }

        _logger.LogInformation("Created new SampleModel with ID: {Id}", model.Id);

        return Ok();
    }
}