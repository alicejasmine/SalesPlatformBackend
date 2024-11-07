using Microsoft.AspNetCore.Mvc;

namespace Api.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;

    public SampleController(ILogger<SampleController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetSample")]
    public string Get()
    {
        return "HELLOWORLD";
    }
}