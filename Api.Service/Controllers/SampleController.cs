using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace Api.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;
    private readonly SampleService _sampleService;

    public SampleController(ILogger<SampleController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetSample")]
    public ActionResult<SampleModel> Get()
    {
        var sample = new SampleModel
        {
            Id = Guid.NewGuid(),
            Name = "Sample 1",
            Description = "This is a sample description.",
            Price = 100,
            Created = DateTime.Now,
            Modified = DateTime.Now
        };

        return Ok(sample);
    }

    [HttpPost(Name = "CreateSample")]
    public async ActionResult<SampleModel> Create([FromBody] SampleModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        try
        {
            await 
        }
       //callservice

        _logger.LogInformation("Created new SampleModel with ID: {Id}", model.Id);

        return CreatedAtRoute("GetSample", new { id = model.Id }, model);
    }
}