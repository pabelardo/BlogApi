using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    //[ApiKey] utilização de uma api key como autorização em vez de token
    public IActionResult Get()
    {
        return Ok();
    }
}