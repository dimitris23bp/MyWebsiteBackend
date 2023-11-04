using Microsoft.AspNetCore.Mvc;

namespace MyWebsiteBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class GithubController : ControllerBase
{
    [HttpGet(Name = "GetGithub")]
    public string Get()
    {
        return "Hello World!";
    }
}