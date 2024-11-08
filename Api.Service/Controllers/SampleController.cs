using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using ApplicationServices.Sample;
using Domain.Dto;

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

    [HttpGet(Name = "GetSample")]
    public async Task<ActionResult<SampleModel>> Get(Guid id)
    {
        var sample = await _sampleService.GetSampleByIdAsync(id);

        return Ok(sample);
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
            _logger.LogError("Could not create new sample in database", ex);
        }

        _logger.LogInformation("Created new SampleModel with ID: {Id}", model.Id);

        return Ok();
    }
}